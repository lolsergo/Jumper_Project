using UnityEngine;

public class ObjectPoolRoot
{
    public Transform Transform { get; }

    public ObjectPoolRoot(Transform transform)
    {
        Transform = transform;
    }
}
