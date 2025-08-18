using UnityEngine;
using Zenject;

public class Obstacle : LevelObject
{
    [SerializeField]
    private int _damage = 1;
    private PlayerHealth _playerHealth;

    [Inject]
    private void Construct(PlayerHealth playerHealth)
    {
        _playerHealth = playerHealth;
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
        if (!gameObject.activeSelf || !other.CompareTag("Player")) return;

        _playerHealth.TakeDamage(_damage);
        _speedManager.DecreaseGameSpeed();
    }
}
