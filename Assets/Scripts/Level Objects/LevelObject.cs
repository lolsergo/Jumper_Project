using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public abstract class LevelObject : MonoBehaviour
{
    public class Factory : PlaceholderFactory<LevelObject> { }

    protected Collider2D _collider;
    protected SpriteRenderer _renderer;

    protected GameSpeedManager _speedManager;
    public event System.Action OnDeactivated;
    public GameObject OriginalPrefab { get; set; }

    [Inject]
    private void Construct(GameSpeedManager speedManager)
    {
        _speedManager = speedManager;
    }

    protected virtual void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        if (gameObject.activeSelf)
        {
            transform.position += Vector3.left * (_speedManager.GameSpeed * Time.deltaTime);
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