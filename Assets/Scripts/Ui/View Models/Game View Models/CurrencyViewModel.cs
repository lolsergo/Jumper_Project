using MVVM;
using System;
using UniRx;
using Zenject;

public sealed class CurrencyViewModel : IInitializable, IDisposable, IObserver<string>
{
    [Data("Gold")]
    public readonly ReactiveProperty<string> Currency = new ReactiveProperty<string>();

    private readonly PlayerCurrency _playerCurrency;
    private IDisposable _subscription;

    public CurrencyViewModel(PlayerCurrency playerCurrency)
    {
        _playerCurrency = playerCurrency;
    }

    public void Initialize()
    {
        // Подписка через ReactiveProperty
        _subscription = _playerCurrency.Gold
            .Select(gold => gold.ToString())
            .Subscribe(this);
    }

    public void Dispose()
    {
        _subscription?.Dispose();
    }

    // Реализация IObserver<string>
    public void OnNext(string value) => Currency.Value = value;
    public void OnCompleted() { }
    public void OnError(Exception error) { }
}
