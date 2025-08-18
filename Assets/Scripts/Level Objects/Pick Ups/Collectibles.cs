using UnityEngine;
using Zenject;

public class Collectibles : LevelObject
{
    public event System.Action OnCollected;

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