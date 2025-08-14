using UnityEngine;
using Zenject;

public abstract class LevelObject : MonoBehaviour
{
    protected GameSpeedManager _speedManager;    
    public bool IsActive { get; private set; }

    [Inject]
    private void Construct(GameSpeedManager speedManager)
    {
        _speedManager = speedManager;
    }

    protected virtual void Update()
    {
        if (!IsActive)
        {
            Debug.Log($"{gameObject.name} is not active");
            return;
        }

        float moveAmount = -_speedManager.GameSpeed * Time.deltaTime;

        // ƒвигаем в мировых координатах (игнориру€ локальный поворот)
        transform.Translate(moveAmount, 0, 0, Space.World);

        if (IsBehindCamera())
            Deactivate();
    }

    public virtual void Activate(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
        IsActive = true;
    }

    public virtual void Deactivate()
    {
        gameObject.SetActive(false);
        IsActive = false;
    }

    private bool IsBehindCamera()
    {
        return transform.position.x < Camera.main.ViewportToWorldPoint(Vector3.zero).x - 5f;
    }

    public class LevelObjectFactory : PlaceholderFactory<LevelObject> { }
}