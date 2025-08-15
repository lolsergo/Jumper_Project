using UnityEngine;
using Zenject;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float _health;
    [SerializeField] private float _invincibilityTime = 1f; // ¬рем€ неу€звимости после удара
    [SerializeField] private float _flashSpeed = 0.1f; // „астота мерцани€

    private SpriteRenderer _sprite;
    private bool _isInvincible = false;

    public float Health { get => _health; private set => _health = value; }
    private GameManager _gameManager;

    public event System.Action OnDeath;
    public event System.Action<float> OnHealthChanged;

    [Inject]
    private void Construct(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage()
    {
        if (_isInvincible) return;

        Health--;
        OnHealthChanged?.Invoke(Health / _health);

        StartCoroutine(InvincibilityFlash());

        if (Health <= 0)
        {
            Die();
        }
    }

    private IEnumerator InvincibilityFlash()
    {
        _isInvincible = true;
        float timer = 0;

        while (timer < _invincibilityTime)
        {
            _sprite.enabled = !_sprite.enabled;
            yield return new WaitForSeconds(_flashSpeed);
            timer += _flashSpeed;
        }

        _sprite.enabled = true;
        _isInvincible = false;
    }

    public void Die()
    {
        Debug.Log("DIED");
        OnDeath?.Invoke();
        _gameManager.GameOver();
    }
}