using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private float _spawnX = 15f;
    [SerializeField] private float _minY = -3f;
    [SerializeField] private float _maxY = 3f;

    public float MinY => _minY;
    public float MaxY => _maxY;

    public Vector3 GetRandomPosition()
    {
        return GetPositionWithY(Random.Range(_minY, _maxY));
    }

    public Vector3 GetPositionWithY(float yPos)
    {
        return new Vector3(_spawnX, Mathf.Clamp(yPos, _minY, _maxY), 0f);
    }
}
