using MVVM;
using System;
using UniRx;
using Zenject;

public class HeartsViewModel : IInitializable, IDisposable
{
    [Data("Health")]
    public readonly ReactiveProperty<int> Health = new();

    private readonly PlayerHealth playerHealth;
    private IDisposable _healthSubscription;

    public HeartsViewModel(PlayerHealth playerHealth)
    {
        this.playerHealth = playerHealth;
    }

    public void Initialize()
    {
        OnHealthChanged(playerHealth.CurrentHealth.Value);
        _healthSubscription = playerHealth.CurrentHealth.Subscribe(OnHealthChanged);
    }

    public void Dispose()
    {
        _healthSubscription?.Dispose();
    }

    private void OnHealthChanged(int health)
    {
        this.Health.Value = health;
    }
}