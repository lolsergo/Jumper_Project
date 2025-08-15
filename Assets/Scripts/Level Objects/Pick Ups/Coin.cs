using UnityEngine;
using Zenject;

public class Coin : LevelObject
{
    [SerializeField] private int _value = 1;
    private Collider2D _collider;
    private SpriteRenderer _renderer;
    private PlayerCurrency _currency;

    [Inject]
    private void Construct(PlayerCurrency currency)
    {
        _currency = currency;
    }

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    public override void Activate(Vector3 position)
    {
        base.Activate(position);
        _collider.enabled = true;
        _renderer.enabled = true;
    }

    public override void Deactivate()
    {
        _collider.enabled = false;
        _renderer.enabled = false;
        base.Deactivate();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!gameObject.activeSelf) return;
        _currency.IncreaseGold(_value);
        Deactivate();
    }
}
