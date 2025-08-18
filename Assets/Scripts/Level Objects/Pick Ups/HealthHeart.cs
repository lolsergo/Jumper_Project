using UnityEngine;
using Zenject;

public class HealthHeart : Collectibles
{
    [SerializeField]
    private int _healthGranted = 1;
    private PlayerHealth _playerHealth;

    [Inject]
    private void Construct(PlayerHealth playerHealth)
    {
        _playerHealth = playerHealth;
    }

    protected override void ApplyEffect()
    {
        _playerHealth.Heal(_healthGranted);
    }
}
