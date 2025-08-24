using UnityEngine;
using Zenject;

public class LevelInstaller : MonoInstaller
{
    [Header("References")]
    [SerializeField] private Transform _startPoint;
    [SerializeField] private GameObject _characterPrefab;
    [SerializeField] private SpawnPoint _spawnPoint;
    [SerializeField] private SpawnConfigSO _spawnConfig;
    [SerializeField] private UIGameServise _uiManager;
    [SerializeField] private PriceConfigSO _priceConfig;

    [Inject] private SceneInjectionHandler _sceneInjectionHandler;
    [Inject] private PlayerProvider _playerProvider;
    [Inject] private DiContainer _sceneContainer;

    private ObjectPoolRoot _poolRoot;

    public override void InstallBindings()
    {
        BindEventBus();
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
        Container.BindInterfacesAndSelfTo<GameplayService>().AsSingle();
        Container.BindInterfacesAndSelfTo<RewardedReviveController>().AsSingle();
        Container.Bind<GameSpeedProvider>().FromComponentInHierarchy().AsSingle();
    }

    private void BindPlayer()
    {
        Container.BindInterfacesAndSelfTo<PlayerController>()
            .FromComponentInNewPrefab(_characterPrefab)
            .AsSingle()
            .OnInstantiated<PlayerController>((ctx, player) =>
            {
                player.transform.SetPositionAndRotation(_startPoint.position, Quaternion.identity);
                Container.InstantiateComponent<CleanupHandler>(player.gameObject);

                var playerHealth = player.GetComponent<PlayerHealth>();
                _playerProvider.SetPlayer(playerHealth);
            });

        Container.Bind<PlayerHealth>()
            .FromResolveGetter<PlayerController>(pc => pc.GetComponent<PlayerHealth>())
            .AsSingle();

        Container.Bind<PlayerCurrency>()
            .FromResolveGetter<PlayerController>(pc => pc.GetComponent<PlayerCurrency>())
            .AsSingle();
    }

    private void BindObjectSystem()
    {
        _poolRoot = CreatePoolRoot();
        Container.Bind<ObjectPoolRoot>().FromInstance(_poolRoot).AsSingle();

        BindLevelObjectFactories(_poolRoot);
        BindLevelObjectGenerator(_poolRoot);
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

    private ObjectPoolRoot CreatePoolRoot()
    {
        var poolRootGO = new GameObject("ObjectPoolRoot");
        Object.DontDestroyOnLoad(poolRootGO);
        return new ObjectPoolRoot(poolRootGO.transform);
    }

    private void BindLevelObjectFactories(ObjectPoolRoot poolRoot)
    {
        foreach (var config in _spawnConfig.spawnableObjects)
        {
            if (config.prefab == null) continue;

            Container.BindFactory<LevelObject, LevelObject.Factory>()
                .WithId(config.prefab.name)
                .FromComponentInNewPrefab(config.prefab)
                .UnderTransform(poolRoot.Transform)
                .OnInstantiated<LevelObject>((ctx, obj) =>
                {
                    obj.gameObject.SetActive(false);
                    obj.OriginalPrefab = config.prefab;
                    _sceneInjectionHandler.RegisterPersistent(obj);
                })
                .NonLazy();
        }
    }

    private void BindLevelObjectGenerator(ObjectPoolRoot poolRoot)
    {
        var genGO = new GameObject("LevelObjectGenerator");
        Object.DontDestroyOnLoad(genGO);
        var gen = Container.InstantiateComponent<LevelObjectGenerator>(genGO);
        Container.Bind<LevelObjectGenerator>().FromInstance(gen).AsSingle();
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
            _fsm.ChangeState(new GameplayState(_eventBus));
            _eventBus.Publish(new GameplayStartedEvent());
        }
    }
}