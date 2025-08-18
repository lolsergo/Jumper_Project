using UniRx;
using UnityEngine;
using System.Collections;
using Zenject;

public class PlayerHealth : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private int _maxLives = 3;
    [SerializeField]
    private float _invincibilityTime = 1f;
    [SerializeField]
    private float _flashSpeed = 0.1f;
    private readonly int _healthOnRessurect = 1;
    public int HealthOnRessurect => _healthOnRessurect;

    public ReactiveProperty<int> CurrentHealth { get; private set; }
    public int MaxHealth => _maxLives;
    private bool _isInvincible;
    private SpriteRenderer _spriteRenderer;

    // Добавляем Subject для уведомления о смерти
    public Subject<Unit> OnDeath = new ();

    [Inject]
    private void Construct()
    {
        CurrentHealth = new ReactiveProperty<int>(_maxLives);
    }

    private void Awake() => _spriteRenderer = GetComponent<SpriteRenderer>();

    public void TakeDamage(int damage)
    {
        if (_isInvincible || CurrentHealth.Value <= 0) return;

        CurrentHealth.Value = Mathf.Max(0, CurrentHealth.Value - damage);
        StartCoroutine(InvincibilityFlash());

        if (CurrentHealth.Value <= 0) Die();
    }

    public void Heal(int amount)
    {
        CurrentHealth.Value = Mathf.Min(_maxLives, CurrentHealth.Value + amount);
    }

    private IEnumerator InvincibilityFlash()
    {
        _isInvincible = true;
        float timer = 0;

        while (timer < _invincibilityTime)
        {
            _spriteRenderer.enabled = !_spriteRenderer.enabled;
            yield return new WaitForSeconds(_flashSpeed);
            timer += _flashSpeed;
        }

        _spriteRenderer.enabled = true;
        _isInvincible = false;
    }

    private void Die()
    {
        // Уведомляем подписчиков о смерти
        OnDeath.OnNext(Unit.Default);
    }
}