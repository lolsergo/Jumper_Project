using System;

public interface IPlayerHealthProvider
{
    int CurrentLives { get; }
    bool IsDead { get; }
    void Heal(int amount);
    IObservable<int> LivesChanged { get; }
}
