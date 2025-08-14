using UnityEngine;

[CreateAssetMenu(fileName = "LevelObjectData", menuName = "Scriptable Objects/LevelObjectData")]
public class LevelObjectData : ScriptableObject
{
    public GameObject _prefab;
    public GameObject Prefab { get => _prefab; private set => _prefab = value; }

    public float _spawnWeight = 50f;
    public float SpawnWeight { get => _spawnWeight; private set => _spawnWeight = value; }

    public bool _canSpawnRepeatedly = true;
    public bool CanSpawnRepeatedly { get => _canSpawnRepeatedly; private set => _canSpawnRepeatedly = value; }
}
