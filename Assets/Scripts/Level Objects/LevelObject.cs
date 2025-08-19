using UnityEngine;
using Zenject;

public abstract class LevelObject : MonoBehaviour
{
    // Вложенная фабрика Zenject
    public class Factory : PlaceholderFactory<LevelObject> { }

    protected Collider2D _collider;
    protected SpriteRenderer _renderer;

    [InjectOptional]
    protected GameSpeedManager _speedManager;

    public event System.Action OnDeactivated;
    public GameObject OriginalPrefab { get; set; }

    protected virtual void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        if (!gameObject.activeSelf || _speedManager == null) return;

        transform.position += Vector3.left * (_speedManager.GameSpeed * Time.deltaTime);

        if (transform.position.x < -20f)
        {
            Deactivate();
        }
    }

    public virtual void Activate(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
    }

    public virtual void Deactivate()
    {
        gameObject.SetActive(false);
        OnDeactivated?.Invoke();
    }
}