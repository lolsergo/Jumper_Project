using UnityEngine;
using Zenject;
using UniRx;

public class LevelInstaller : MonoInstaller
{
    [Header("References")]
    [SerializeField] private Transform _startPoint;
    [SerializeField] private GameObject _characterPrefab;
    [SerializeField] private SpawnPoint _spawnPoint;
    [SerializeField] private SpawnConfigSO _spawnConfig;
    [SerializeField] private UIGameManager _uiManager;
    [SerializeField] private PriceConfigSO _priceConfig;

    [Inject] private SceneInjectionHandler _sceneInjectionHandler;
    [Inject] private PlayerProvider _playerProvider;
    [Inject] private DiContainer _sceneContainer;

    public override void InstallBindings()
    {
        BindEventBus(); // <-- Добавьте это первым!
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
        Container.BindInterfacesAndSelfTo<PlayerHealthService>().AsSingle();
        Container.BindInterfacesAndSelfTo<MoneyService>().AsSingle();
        Container.BindInstance(_uiManager).AsSingle();
        Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<RewardedReviveController>().AsSingle();
        Container.Bind<GameSpeedManager>().FromComponentInHierarchy().AsSingle();
    }

    private void BindPlayer()
    {
        // PlayerController will be instantiated only when first resolved (or by something NonLazy after install)
        Container.BindInterfacesAndSelfTo<PlayerController>()
            .FromComponentInNewPrefab(_characterPrefab)
            .AsSingle()
            .OnInstantiated<PlayerController>((ctx, player) =>
            {
                player.transform.SetPositionAndRotation(_startPoint.position, Quaternion.identity);
                Container.InstantiateComponent<CleanupHandler>(player.gameObject); // <-- исправлено

                var playerHealth = player.GetComponent<PlayerHealth>();
                _playerProvider.SetPlayer(playerHealth);
            });

        // Expose PlayerHealth & PlayerCurrency via getters so they are resolved after the player exists
        Container.Bind<PlayerHealth>()
            .FromResolveGetter<PlayerController>(pc => pc.GetComponent<PlayerHealth>())
            .AsSingle();

        Container.Bind<PlayerCurrency>()
            .FromResolveGetter<PlayerController>(pc => pc.GetComponent<PlayerCurrency>())
            .AsSingle();
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

    private void BindEventBus()
    {
        Container.Bind<IEventBus>().To<EventBus>().AsSingle();
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
        var existingGenerator = GameObject.Find("LevelObjectGenerator");
        LevelObjectGenerator gen;

        if (existingGenerator != null)
        {
            gen = existingGenerator.GetComponent<LevelObjectGenerator>();
            gen.DeactivateAllObjects();
            Container.Inject(gen); // инъекция для уже существующего экземпляра
            Container.Bind<LevelObjectGenerator>().FromInstance(gen).AsSingle();
        }
        else
        {
            var genGO = new GameObject("LevelObjectGenerator");
            Object.DontDestroyOnLoad(genGO);
            gen = Container.InstantiateComponent<LevelObjectGenerator>(genGO);
            Container.Bind<LevelObjectGenerator>().FromInstance(gen).AsSingle();
        }
    }

    private class LevelInitializer : IInitializable
    {
        private readonly GameStateMachine _fsm;
        private readonly IEventBus _eventBus;

        public LevelInitializer(GameStateMachine fsm, IEventBus eventBus)
        {
            _fsm = fsm;
            _eventBus = eventBus;
        }

        public void Initialize()
        {
            _fsm.ChangeState(new GameplayState(_fsm, _eventBus));
            _eventBus.Publish(new GameplayStartedEvent());
        }
    }
}