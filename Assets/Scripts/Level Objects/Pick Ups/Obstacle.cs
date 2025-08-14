using UnityEngine;
using Zenject;

public class Obstacle : LevelObject
{
    // 2. Компоненты
    [Inject(Optional = true)]
    private readonly Collider2D _collider;
    [Inject(Optional = true)]
    private readonly SpriteRenderer _spriteRenderer;
    protected PlayerHealth _playerHealth;

    [Inject]
    private void Construct(PlayerHealth playerHealth)
    {
        _playerHealth = playerHealth;
    }

    // 4. Активация препятствия (переопределяем базовый метод)
    public override void Activate(Vector3 position)
    {
        base.Activate(position); // Вызываем базовую активацию

        transform.Rotate(0, 0, Random.Range(0f, 360f));
        if (_collider != null) _collider.enabled = true;
        if (_spriteRenderer != null) _spriteRenderer.enabled = true;
    }

    // 5. Деактивация (возврат в пул)
    public override void Deactivate()
    {
        base.Deactivate(); // Базовая деактивация
        if (_collider != null) _collider.enabled = false; // Выключаем коллайдер
    }

    // 6. Обработка столкновения с игроком
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsActive) return; // Если объект неактивен - игнорируем

        _speedManager.DecreaseGameSpeed();
        _playerHealth.TakeDamage();
    }
}
