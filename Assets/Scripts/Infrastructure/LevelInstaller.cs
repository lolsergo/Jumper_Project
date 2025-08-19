using UnityEngine;
using Zenject;
using UniRx;
using UnityEngine.SceneManagement;

public class LevelInstaller : MonoInstaller
{
    [Header("References")]
    [SerializeField] private Transform _startPoint;
    [SerializeField] private GameObject _characterPrefab;
    [SerializeField] private SpawnPoint _spawnPoint;
    [SerializeField] private SpawnConfigSO _spawnConfig;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private PriceConfigSO _priceConfig;

    [Inject] private SceneInjectionHandler _sceneInjectionHandler;
    [Inject] private PlayerProvider _playerProvider;   // ”бедись, что он биндитс€ в ProjectContext, если нет Ч раскомментируй строку в BindCoreSystems
    [Inject] private DiContainer _sceneContainer;

    public override void InstallBindings()
    {
        _sceneInjectionHandler.SetSceneContainer(_sceneContainer);

        BindConfiguration();
        BindCoreSystems();
        BindPlayer();
        BindObjectSystem();
        BindStateMachineAndInput();
        BindSceneCleanupBroadcaster();
    }

    private void BindConfiguration()
    {
        Container.BindInstance(_spawnConfig).AsSingle();
        Container.BindInstance(_spawnPoint).AsSingle();
        Container.BindInstance(_priceConfig).AsSingle();
    }

    private void BindCoreSystems()
    {
        // ≈сли PlayerProvider не биндитс€ в ProjectContext:
        // Container.Bind<PlayerProvider>().AsSingle();

        Container.BindInterfacesAndSelfTo<PlayerHealthService>().AsSingle();

        // ƒеньги Ч один единственный биндинг
        Container.BindInterfacesAndSelfTo<MoneyService>().AsSingle();

        Container.BindInstance(_uiManager).AsSingle();
        Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();
        Container.Bind<GameSpeedManager>().FromComponentInHierarchy().AsSingle();
    }

    private void BindPlayer()
    {
        var player = Container.InstantiatePrefabForComponent<PlayerController>(
            _characterPrefab,
            _startPoint.position,
            Quaternion.identity,
            null
        );

        player.gameObject.AddComponent<CleanupHandler>();

        var playerHealth = player.GetComponent<PlayerHealth>();
        var playerCurrency = player.GetComponent<PlayerCurrency>();

        _playerProvider.SetPlayer(playerHealth);

        Container.BindInterfacesAndSelfTo<PlayerController>().FromInstance(player).AsSingle();
        Container.Bind<PlayerHealth>().FromInstance(playerHealth).AsSingle();
        Container.Bind<PlayerCurrency>().FromInstance(playerCurrency).AsSingle();
    }

    private void BindObjectSystem()
    {
        Transform poolRoot = GetOrCreatePoolRoot();
        Container.Bind<Transform>().WithId("ObjectPoolRoot").FromInstance(poolRoot).AsSingle();

        // ‘абрики
        foreach (var config in _spawnConfig.spawnableObjects)
        {
            if (config.prefab == null) continue;

            Container.BindFactory<LevelObject, LevelObject.Factory>()
                .WithId(config.prefab.name)
                .FromComponentInNewPrefab(config.prefab)
                .UnderTransform(poolRoot)
                .OnInstantiated<LevelObject>((ctx, obj) =>
                {
                    obj.gameObject.SetActive(false);
                    obj.OriginalPrefab = config.prefab;
                    _sceneInjectionHandler.RegisterPersistent(obj);
                })
                .NonLazy();
        }

        // √енератор
        var existingGenerator = poolRoot.GetComponentInChildren<LevelObjectGenerator>();
        if (existingGenerator != null)
        {
            existingGenerator.DeactivateAllObjects();
            Container.Bind<LevelObjectGenerator>().FromInstance(existingGenerator).AsSingle();
        }
        else
        {
            var genGO = new GameObject("LevelObjectGenerator");
            genGO.transform.SetParent(poolRoot);
            Object.DontDestroyOnLoad(genGO);

            var gen = genGO.AddComponent<LevelObjectGenerator>();
            Container.Bind<LevelObjectGenerator>().FromInstance(gen).AsSingle();
            Container.Inject(gen);
        }
    }

    private void BindStateMachineAndInput()
    {
        Container.Bind<GameStateMachine>().AsSingle();
        Container.BindInterfacesTo<SceneInputSchemeHandler>().AsSingle().NonLazy();
        Container.BindInterfacesTo<PauseInputHandler>().AsSingle().NonLazy();
        Container.BindInterfacesTo<LevelInitializer>().AsSingle().NonLazy();
    }

    private void BindSceneCleanupBroadcaster()
    {
        Container.BindInterfacesTo<SceneCleanupBroadcaster>().AsSingle().NonLazy();
    }

    private Transform GetOrCreatePoolRoot()
    {
        var existingRoot = GameObject.Find("ObjectPoolRoot");
        if (existingRoot != null)
        {
            Object.DontDestroyOnLoad(existingRoot);
            return existingRoot.transform;
        }

        var poolRootGO = new GameObject("ObjectPoolRoot");
        Object.DontDestroyOnLoad(poolRootGO);
        return poolRootGO.transform;
    }

    private class LevelInitializer : IInitializable
    {
        private readonly GameStateMachine _fsm;
        public LevelInitializer(GameStateMachine fsm) => _fsm = fsm;

        public void Initialize()
        {
            _fsm.ChangeState(new GameplayState(_fsm));
            GameEvents.OnGameplayStarted.OnNext(Unit.Default);
        }
    }
}