using System;
using UniRx;
using Zenject;

public class MoneyService : IMoneyService, IInitializable, IDisposable
{
    private PlayerCurrency _playerCurrency;

    public IReadOnlyReactiveProperty<int> CurrentMoney => _playerCurrency.Gold;

    [Inject]
    public MoneyService(PlayerCurrency playerCurrency)
    {
        _playerCurrency = playerCurrency;
    }

    public void Initialize()
    {
        // ���� ����� � �������� �� ��������� ��� UI, ����� � �.�.
        _playerCurrency.Gold
            .Subscribe(value =>
            {
            })
            .AddTo(_disposables);
    }

    public void AddMoney(int amount)
    {
        _playerCurrency.IncreaseGold(amount);
    }

    public bool TrySpend(int amount)
    {
        if (_playerCurrency.Gold.Value >= amount)
        {
            // ������� ReduceGold ���, ����� ������� ��������� �����
            _playerCurrency.DeacreaseGold(amount);
            return true;
        }
        return false;
    }

    public void Dispose()
    {
        _disposables.Dispose();
    }

    private readonly CompositeDisposable _disposables = new();
}