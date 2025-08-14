using UnityEngine;
using Zenject;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private float _health;
    public float Health { get => _health; private set => _health = value; }
    private GameManager _gameManager;

    public event System.Action OnDeath;
    public event System.Action<float> OnHealthChanged;

    [Inject]
    private void Construct(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void TakeDamage()
    {
        Health--;
        OnHealthChanged?.Invoke(Health / _health);

        if (Health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("DIED");
        OnDeath?.Invoke();
        _gameManager.GameOver();
    }
}
