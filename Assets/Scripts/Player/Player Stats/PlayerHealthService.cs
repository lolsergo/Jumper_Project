using Zenject;
using UniRx;
using System;

public class PlayerHealthService : IHealthService, IInitializable, IDisposable
{
    private readonly PlayerProvider _playerProvider;

    private readonly Subject<Unit> _onDeath = new();
    private CompositeDisposable _playerDisposables = new();

    public IReadOnlyReactiveProperty<int> CurrentHealth { get; private set; }
    public IObservable<Unit> OnDeath => _onDeath;

    [Inject]
    public PlayerHealthService(PlayerProvider playerProvider)
    {
        _playerProvider = playerProvider;
    }

    public void Initialize()
    {
        _playerProvider.PlayerSet += OnPlayerSet;

        if (_playerProvider.PlayerHealth != null)
            HookPlayer(_playerProvider.PlayerHealth);
    }

    private void OnPlayerSet(PlayerHealth newPlayer)
    {
        _playerDisposables.Clear();

        if (newPlayer != null)
            HookPlayer(newPlayer);
        else
            CurrentHealth = null;
    }

    private void HookPlayer(PlayerHealth player)
    {
        CurrentHealth = player.CurrentHealth;

        player.OnDeath
            .Subscribe(_ =>
            {
                UnityEngine.Debug.Log("PlayerHealthService: death caught");
                _onDeath.OnNext(Unit.Default); // ретрансляция каждой смерти
            })
            .AddTo(_playerDisposables);
    }

    public void AddHealth(int amount) => _playerProvider.PlayerHealth?.Heal(amount);
    public void TakeDamage(int amount) => _playerProvider.PlayerHealth?.TakeDamage(amount);

    public void Dispose()
    {
        _playerProvider.PlayerSet -= OnPlayerSet;
        _playerDisposables.Dispose();
        _onDeath.Dispose();
    }
}