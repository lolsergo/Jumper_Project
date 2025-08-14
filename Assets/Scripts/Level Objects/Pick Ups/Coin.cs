using UnityEngine;
using Zenject;

public class Coin : LevelObject
{
    // 2. Компоненты (инъекция)
    [Inject(Optional = true)]
    private readonly Collider2D _collider;
    [Inject(Optional = true)]
    private readonly SpriteRenderer _spriteRenderer;
    [SerializeField]
    private int goldGranted = 1;

    protected PlayerCurrency _playerCurrency;

    [Inject]
    private void Construct(PlayerCurrency playerCurrency)
    {
        _playerCurrency = playerCurrency;
    }

    // 3. Активация (из пула)
    public override void Activate(Vector3 position)
    {
        base.Activate(position);
        if (_collider != null) _collider.enabled = true;
        if (_spriteRenderer != null) _spriteRenderer.enabled = true;
    }

    // 4. Деактивация (в пул)
    public override void Deactivate()
    {
        base.Deactivate();
        if (_collider != null) _collider.enabled = false;
    }

    // 5. Логика сбора (без PlayerWallet)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsActive) return;

        _playerCurrency.IncreaseGold(goldGranted);
        Deactivate();
    }
}
