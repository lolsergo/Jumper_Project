using UnityEngine;

public class LevelObjectPool : MonoBehaviour
{
    private Transform _poolRoot;

    public void Initialize()
    {
        _poolRoot = new GameObject("ObjectPool").transform;
        _poolRoot.SetParent(null);
        _poolRoot.position = Vector3.zero;
    }

    public Transform PoolRoot => _poolRoot;

    public void ClearPool()
    {
        if (_poolRoot != null)
        {
            Destroy(_poolRoot.gameObject);
        }
    }
}
