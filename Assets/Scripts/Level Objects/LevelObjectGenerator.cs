using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Zenject;

public class LevelObjectGenerator : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private float _travelDistanceBetweenSpawn = 10f;
    [SerializeField] private float _despawnX = -15f;

    // Инжектируемые зависимости
    private GameSpeedManager _speedManager;
    private SpawnConfigSO _spawnConfig;
    private SpawnPoint _spawnPoint;
    private Transform _poolRoot;

    private float _distanceSinceLastSpawn;
    private readonly Dictionary<GameObject, Queue<LevelObject>> _pools = new();
    private readonly List<LevelObject> _activeObjects = new();

    [Inject]
    private readonly DiContainer _container;

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

    private void InitializePools()
    {
        if (_spawnConfig == null || _spawnConfig.spawnableObjects == null)
        {
            Debug.LogError("SpawnConfig not set or invalid!");
            return;
        }

        foreach (var config in _spawnConfig.spawnableObjects)
        {
            if (config.prefab == null)
            {
                Debug.LogWarning("Missing prefab in SpawnConfig!");
                continue;
            }

            var pool = new Queue<LevelObject>();
            for (int i = 0; i < 5; i++)
            {
                var factory = _container.ResolveId<LevelObject.Factory>(config.prefab.name);
                var levelObj = factory.Create();

                levelObj.transform.SetParent(_poolRoot);
                levelObj.OriginalPrefab = config.prefab;
                levelObj.OnDeactivated += () => ReturnToPool(levelObj);
                levelObj.gameObject.SetActive(false);
                pool.Enqueue(levelObj);
            }
            _pools[config.prefab] = pool;
        }
    }

    private void Update()
    {
        _distanceSinceLastSpawn += _speedManager.GameSpeed * Time.deltaTime;

        if (_distanceSinceLastSpawn >= _travelDistanceBetweenSpawn)
        {
            SpawnObject();
            _distanceSinceLastSpawn = 0f;
        }

        CheckDespawn();
    }

    private void SpawnObject()
    {
        var prefab = GetRandomPrefab();
        if (prefab == null) return;

        var obj = GetObjectFromPool(prefab);
        if (obj != null)
        {
            obj.Activate(_spawnPoint.GetRandomPosition());
            _activeObjects.Add(obj);
        }
    }

    private LevelObject GetObjectFromPool(GameObject prefab)
    {
        if (!_pools.TryGetValue(prefab, out var pool) || pool.Count == 0)
        {
            return CreateNewObject(prefab);
        }
        return pool.Dequeue();
    }

    private LevelObject CreateNewObject(GameObject prefab)
    {
        // Получаем фабрику по идентификатору
        var factory = _container.ResolveId<LevelObject.Factory>(prefab.name);
        var levelObj = factory.Create();

        levelObj.transform.SetParent(_poolRoot);
        levelObj.OriginalPrefab = prefab;
        levelObj.OnDeactivated += () => ReturnToPool(levelObj);
        return levelObj;
    }

    private GameObject GetRandomPrefab()
    {
        if (_spawnConfig == null || _spawnConfig.spawnableObjects == null)
            return null;

        float totalWeight = 0f;
        foreach (var config in _spawnConfig.spawnableObjects)
        {
            if (config.prefab != null)
                totalWeight += config.spawnWeight;
        }

        if (totalWeight <= 0f)
            return null;

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
            if (_activeObjects[i] == null ||
                _activeObjects[i].transform.position.x < _despawnX)
            {
                if (_activeObjects[i] != null)
                    _activeObjects[i].Deactivate();
                _activeObjects.RemoveAt(i);
            }
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
}