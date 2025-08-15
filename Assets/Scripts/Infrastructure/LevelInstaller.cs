using UnityEngine;
using Zenject;

[System.Serializable]
public class GameReferences
{
    public SpawnPoint SpawnPoint;
    public GameManager GameManager;
    public GameSpeedManager SpeedManager;
    public InputController InputController;
    public SpawnConfigSO SpawnConfig;
}

public class LevelInstaller : MonoInstaller
{
    [Header("References")]
    [SerializeField] private GameReferences _references;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private GameObject _characterPrefab;

    public override void InstallBindings()
    {
        BindConfiguration();
        InitializeCoreSystems();
        InitializePlayer();
        InitializeObjectSystem();
    }

    private void BindConfiguration()
    {
        Container.BindInstance(_references.SpawnConfig);
        Container.BindInstance(_references.SpawnPoint);
    }

    private void InitializeCoreSystems()
    {
        Container.BindInterfacesAndSelfTo<GameManager>()
            .FromInstance(_references.GameManager)
            .AsSingle();

        Container.BindInterfacesAndSelfTo<GameSpeedManager>()
            .FromInstance(_references.SpeedManager)
            .AsSingle();

        Container.BindInterfacesAndSelfTo<InputController>()
            .FromInstance(_references.InputController)
            .AsSingle()
            .NonLazy();
    }

    private void InitializePlayer()
    {
        var player = Container.InstantiatePrefabForComponent<PlayerController>(
            _characterPrefab,
            _startPoint.position,
            Quaternion.identity,
            null);

        Container.BindInterfacesAndSelfTo<PlayerController>()
            .FromInstance(player)
            .AsSingle();

        Container.Bind<PlayerHealth>()
            .FromComponentOn(player.gameObject)
            .AsSingle();

        Container.Bind<PlayerCurrency>()
            .FromComponentOn(player.gameObject)
            .AsSingle();
    }

    private void InitializeObjectSystem()
    {
        // —оздаем корневой объект дл€ пула
        var poolRoot = new GameObject("ObjectPoolRoot").transform;
        poolRoot.SetParent(null);
        DontDestroyOnLoad(poolRoot);

        // –егистрируем зависимости
        Container.Bind<Transform>()
            .WithId("ObjectPoolRoot")
            .FromInstance(poolRoot)
            .AsSingle();

        // явно регистрируем фабрику дл€ базового LevelObject
        Container.BindFactory<LevelObject, LevelObject.Factory>()
            .FromMethod(container =>
            {
                // Ёта часть никогда не выполнитс€, так как мы используем FromComponentInNewPrefab
                return null;
            });

        // —оздаем и регистрируем генератор
        Container.Bind<LevelObjectGenerator>()
            .FromNewComponentOnNewGameObject()
            .UnderTransform(poolRoot)
            .AsSingle()
            .NonLazy();

        // –егистрируем фабрики дл€ конкретных объектов
        foreach (var config in _references.SpawnConfig.spawnableObjects)
        {
            Container.BindFactory<LevelObject, LevelObject.Factory>()
                .WithId(config.prefab.name)
                .FromComponentInNewPrefab(config.prefab)
                .UnderTransform(poolRoot)
                .NonLazy();
        }
    }
}