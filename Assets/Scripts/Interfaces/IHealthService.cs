using System;
using UniRx;

public interface IHealthService
{
    IReadOnlyReactiveProperty<int> CurrentHealth { get; }
    IObservable<Unit> OnDeath { get; }
    void AddHealth(int amount);
    void TakeDamage(int amount);
}
