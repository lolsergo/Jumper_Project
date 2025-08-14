using UnityEngine;
using Zenject;

[System.Serializable]
public class GameReferences
{
    public GameManager GameManager;
    public GameSpeedManager SpeedManager;
    public LevelObjectGenerator ObjectGenerator;
    public InputController InputController;
}

public class LevelInstaller : MonoInstaller
{
    [SerializeField]
    private Transform _poolParent;
    [SerializeField]
    private Transform _startPoint;
    [SerializeField]
    private GameObject _characterPrefab;
    [SerializeField]
    private GameReferences _references;

    public override void InstallBindings()
    {
        InitializeInput();
        InitializeManagers();
        InitializeObjectSpawnSystem();
        InitializePlayer();
    }

    private void InitializePlayer()
    {
        var player = Container.InstantiatePrefabForComponent<PlayerController>(
            _characterPrefab,
            _startPoint.position,
            Quaternion.identity,
            null);

        Container.Bind<PlayerController>()
            .FromInstance(player)
            .AsSingle();

        Container.Bind<PlayerHealth>()
            .FromComponentOn(player.gameObject)
            .AsSingle();

        Container.Bind<PlayerCurrency>()
            .FromComponentOn(player.gameObject)
            .AsSingle();
    }

    private void InitializeObjectSpawnSystem()
    {
        Container.Bind<LevelObjectGenerator>()
            .FromInstance(_references.ObjectGenerator)
            .AsSingle()
            .NonLazy();

        if (_poolParent == null)
        {
            _poolParent = new GameObject("PooledObjects").transform;
            _poolParent.SetParent(transform);
        }

        Container.BindFactory<LevelObject, LevelObject.LevelObjectFactory>()
    .FromMethod(container =>
    {
        var prefab = container.Resolve<LevelObjectGenerator>().GetRandomPrefab();
        return container.InstantiatePrefabForComponent<LevelObject>(
            prefab,
            Vector3.zero,
            Quaternion.identity,
            _poolParent);
    });
    }

    private void InitializeManagers()
    {
        Container.Bind<GameManager>()
            .FromInstance(_references.GameManager)
            .AsSingle();

        Container.Bind<GameSpeedManager>()
            .FromInstance(_references.SpeedManager)
            .AsSingle();
    }

    private void InitializeInput()
    {
        Container.Bind<InputController>()
            .FromInstance(_references.InputController)
            .AsSingle()
            .NonLazy();
    }
}