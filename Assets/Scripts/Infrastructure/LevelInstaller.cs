using UnityEngine;
using Zenject;
using UniRx;

public class LevelInstaller : MonoInstaller
{
    // === Inspector References ===
    [Header("References")]
    [SerializeField] private Transform _startPoint;
    [SerializeField] private GameObject _characterPrefab;
    [SerializeField] private SpawnPoint _spawnPoint;
    [SerializeField] private SpawnConfigSO _spawnConfig;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private PriceConfigSO _priceConfig;

    // === Injected Dependencies ===
    [Inject] private SceneInjectionHandler _sceneInjectionHandler;
    [Inject] private PlayerProvider _playerProvider; // Если не биндится в ProjectContext — раскомментируй в BindCoreSystems
    [Inject] private DiContainer _sceneContainer;

    // === Installer Entry Point ===
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

    // === Bindings ===
    private void BindConfiguration()
    {
        Container.BindInstance(_spawnConfig).AsSingle();
        Container.BindInstance(_spawnPoint).AsSingle();
        Container.BindInstance(_priceConfig).AsSingle();
    }

    private void BindCoreSystems()
    {
        // Container.Bind<PlayerProvider>().AsSingle(); // если нет в ProjectContext

        Container.BindInterfacesAndSelfTo<PlayerHealthService>().AsSingle();
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
        var poolRoot = GetOrCreatePoolRoot();
        Container.Bind<Transform>().WithId("ObjectPoolRoot").FromInstance(poolRoot).AsSingle();

        BindLevelObjectFactories(poolRoot);
        BindLevelObjectGenerator(poolRoot);
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

    // === Helpers ===
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

    private void BindLevelObjectFactories(Transform poolRoot)
    {
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
    }

    private void BindLevelObjectGenerator(Transform poolRoot)
    {
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

    // === Nested Initializer ===
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