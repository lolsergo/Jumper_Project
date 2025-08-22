using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;

public class LevelObjectGenerator : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private float _travelDistanceBetweenSpawn = 10f;
    [SerializeField] private float _despawnX = -15f;

    private GameSpeedManager _speedManager;
    private SpawnConfigSO _spawnConfig;
    private SpawnPoint _spawnPoint;
    private Transform _poolRoot;

    private float _distanceSinceLastSpawn;
    private bool _isActive;

    private readonly Dictionary<GameObject, Queue<LevelObject>> _pools = new();
    private readonly List<LevelObject> _activeObjects = new();

    [Inject] private readonly DiContainer _container;

    [Inject]
    private void Construct(
        GameSpeedManager speedManager,
        SpawnConfigSO spawnConfig,
        SpawnPoint spawnPoint,
        [Inject(Id = "ObjectPoolRoot")] Transform poolRoot)
    {
        _speedManager = speedManager;
        _spawnConfig = spawnConfig;
        _spawnPoint = spawnPoint;
        _poolRoot = poolRoot;

        InitializePools();
    }

    private void OnEnable()
    {
        SubscribeToGameEvents();
    }

    private void Update()
    {
        if (!_isActive) return;

        _distanceSinceLastSpawn += _speedManager.GameSpeed * Time.deltaTime;

        if (_distanceSinceLastSpawn >= _travelDistanceBetweenSpawn)
        {
            SpawnObject();
            _distanceSinceLastSpawn = 0f;
        }

        CheckDespawn();
    }
    private void InitializePools()
    {
        foreach (var config in _spawnConfig.spawnableObjects)
        {
            if (config.prefab == null) continue;
            _pools[config.prefab] = CreatePoolForPrefab(config.prefab, 5);
        }
    }

    private Queue<LevelObject> CreatePoolForPrefab(GameObject prefab, int count)
    {
        var pool = new Queue<LevelObject>();
        for (int i = 0; i < count; i++)
        {
            var levelObj = CreateNewObject(prefab);
            levelObj.gameObject.SetActive(false);
            pool.Enqueue(levelObj);
        }
        return pool;
    }

    private void SpawnObject()
    {
        var prefab = GetRandomPrefab();
        if (prefab == null) return;

        var obj = GetObjectFromPool(prefab);
        if (obj == null) return;

        obj.Activate(_spawnPoint.GetRandomPosition());
        _activeObjects.Add(obj);
    }

    private LevelObject GetObjectFromPool(GameObject prefab)
    {
        if (!_pools.TryGetValue(prefab, out var pool) || pool.Count == 0)
            return CreateNewObject(prefab);

        return pool.Dequeue();
    }

    private LevelObject CreateNewObject(GameObject prefab)
    {
        var factory = _container.ResolveId<LevelObject.Factory>(prefab.name);
        var levelObj = factory.Create();

        levelObj.transform.SetParent(_poolRoot);
        levelObj.OriginalPrefab = prefab;
        levelObj.OnDeactivated += () => ReturnToPool(levelObj);
        return levelObj;
    }

    private GameObject GetRandomPrefab()
    {
        float totalWeight = 0f;
        foreach (var config in _spawnConfig.spawnableObjects)
            if (config.prefab != null)
                totalWeight += config.spawnWeight;

        if (totalWeight <= 0f) return null;

        float random = Random.Range(0f, totalWeight);
        float current = 0f;

        foreach (var config in _spawnConfig.spawnableObjects)
        {
            if (config.prefab == null) continue;
            current += config.spawnWeight;
            if (random <= current)
                return config.prefab;
        }

        return null;
    }

    private void CheckDespawn()
    {
        for (int i = _activeObjects.Count - 1; i >= 0; i--)
        {
            var obj = _activeObjects[i];
            if (obj == null || obj.transform.position.x < _despawnX)
                obj?.Deactivate();
        }
    }

    private void ReturnToPool(LevelObject obj)
    {
        if (obj == null || obj.OriginalPrefab == null)
            return;

        _activeObjects.Remove(obj);

        if (_pools.TryGetValue(obj.OriginalPrefab, out var pool))
        {
            pool.Enqueue(obj);
            obj.gameObject.SetActive(false);
        }
        else
        {
            Destroy(obj.gameObject);
        }
    }

    public void DeactivateAllObjects()
    {
        for (int i = _activeObjects.Count - 1; i >= 0; i--)
            _activeObjects[i]?.Deactivate();

        _activeObjects.Clear();
    }

    public void StopSpawning()
    {
        StopAllCoroutines();
        DeactivateAllObjects();
    }

    private void SubscribeToGameEvents()
    {
        GameEvents.OnGameCleanup
            .Subscribe(_ =>
            {
                _speedManager.ResetSpeed();
                _isActive = false;
                DeactivateAllObjects();
            })
            .AddTo(this);

        GameEvents.OnGameplayStarted
            .Subscribe(_ => _isActive = true)
            .AddTo(this);

        GameEvents.OnGameOver
            .Subscribe(_ =>
            {
                _isActive = false;
                DeactivateAllObjects();
            })
            .AddTo(this);
    }
}