using UnityEngine;
using Zenject;

public class Collectibles : LevelObject
{
    public event System.Action OnCollected;

    public override void Activate(Vector3 position)
    {
        // Включаем коллайдер и рендерер, т.к. Deactivate() их выключает
        if (_collider != null) _collider.enabled = true;
        if (_renderer != null) _renderer.enabled = true;

        base.Activate(position);
    }

    public override void Deactivate()
    {
        _collider.enabled = false;
        _renderer.enabled = false;
        base.Deactivate();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!gameObject.activeSelf || !other.CompareTag("Player")) return;
        Collect();
        Deactivate();
    }

    protected virtual void Collect()
    {
        ApplyEffect();
        OnCollected?.Invoke();
    }

    protected virtual void ApplyEffect()
    {
        // Базовая реализация пустая
    }
}