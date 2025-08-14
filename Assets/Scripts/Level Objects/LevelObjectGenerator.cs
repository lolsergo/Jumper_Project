using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using static LevelObject;

public class LevelObjectGenerator : MonoBehaviour
{
    protected GameSpeedManager _speedManager;

    [Header("Pool Settings")]
    [SerializeField] private int _capacity = 10;

    [Header("Spawn Settings")]
    [SerializeField]
    private List<GameObject> _levelObjects;
    [SerializeField]
    private Transform[] _spawnPoints;
    [SerializeField]
    private float _travelDistanceBetweenSpawn = 10f;

    [Inject]
    private void Construct(GameSpeedManager speedManager)
    {
        _speedManager = speedManager;
    }

    private float _elapsedDistance;
    private List<LevelObject> _pool = new();

    [Inject]
    private LevelObjectFactory _levelObjectFactory;

    public GameObject GetRandomPrefab()
    {
        return _levelObjects[Random.Range(0, _levelObjects.Count)];
    }

    private void Start()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < _capacity; i++)
        {
            CreatePooledObject();
        }
    }

    private LevelObject CreatePooledObject()
    {
        var obj = _levelObjectFactory.Create();
        obj.gameObject.SetActive(false);
        _pool.Add(obj);
        return obj;
    }

    private void Update()
    {
        _elapsedDistance += Time.deltaTime * _speedManager.GameSpeed;

        if (_elapsedDistance >= _travelDistanceBetweenSpawn)
        {
            TrySpawnObject();
            _elapsedDistance = 0;
        }
    }

    private void TrySpawnObject()
    {
        // Получаем все неактивные объекты
        var inactiveObjects = _pool.Where(obj => !obj.gameObject.activeInHierarchy).ToList();

        // Если есть неактивные - выбираем случайный
        if (inactiveObjects.Count > 0)
        {
            SpawnObject(inactiveObjects[Random.Range(0, inactiveObjects.Count)]);
            return;
        }

        // Если все заняты - создаем новый
        SpawnObject(CreatePooledObject());
    }

    private void SpawnObject(LevelObject obj)
    {
        // Получаем случайную позицию спавна
        Vector3 spawnPosition = _spawnPoints[Random.Range(0, _spawnPoints.Length)].position;

        // Активируем объект через его метод Activate()
        obj.Activate(spawnPosition);
    }
}