using UnityEngine;
using Zenject;

public class Obstacle : LevelObject
{
    private Collider2D _collider;
    private SpriteRenderer _renderer;
    private PlayerHealth _playerHealth;

    [Inject]
    private void Construct(PlayerHealth playerHealth)
    {
        _playerHealth = playerHealth;
    }

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    public override void Activate(Vector3 position)
    {
        base.Activate(position);
        // Добавляем случайное вращение по Z
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        _collider.enabled = true;
        _renderer.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!gameObject.activeSelf) return;
        _playerHealth.TakeDamage();
        _speedManager.DecreaseGameSpeed();
    }
}
