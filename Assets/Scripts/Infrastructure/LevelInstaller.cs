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
        // 1. Сначала создаем корень пула
        var poolRoot = new GameObject("ObjectPoolRoot").transform;

        // 2. Регистрируем корень
        Container.Bind<Transform>()
            .WithId("ObjectPoolRoot")
            .FromInstance(poolRoot)
            .AsSingle();

        // 3. Создаем генератор
        Container.Bind<LevelObjectGenerator>()
            .FromNewComponentOnNewGameObject()
            .UnderTransform(poolRoot)
            .AsSingle()
            .NonLazy();

        // 4. Регистрируем фабрики
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