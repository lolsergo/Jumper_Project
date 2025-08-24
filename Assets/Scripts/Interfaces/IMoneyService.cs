using UniRx;

public interface IMoneyService
{
    IReadOnlyReactiveProperty<int> CurrentMoney { get; }
    void AddMoney(int amount);
    bool TrySpend(int amount);
}

