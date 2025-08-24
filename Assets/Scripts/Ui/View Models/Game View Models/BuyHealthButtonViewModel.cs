using System;
using UniRx;
using Zenject;
using UnityEngine;
using MVVM;

public class BuyHealthButtonViewModel : IInitializable, IDisposable
{
    [Data("BuyHealthClick")]
    public readonly Action BuyHealthAction;

    [Data("ButtonColor")]
    public readonly IReadOnlyReactiveProperty<Color> ButtonColor;

    [Data("Price")]
    public readonly IReadOnlyReactiveProperty<int> Price;

    private readonly GameplayService _gameManager;
    private readonly IMoneyService _moneyService;
    private readonly PriceConfigSO _priceConfig;

    private readonly CompositeDisposable _disposables = new();

    private readonly IntReactiveProperty _purchaseCount = new(0);

    [Inject]
    public BuyHealthButtonViewModel(
        GameplayService gameManager,
        IMoneyService moneyService,
        PriceConfigSO priceConfig
    )
    {
        _gameManager = gameManager;
        _moneyService = moneyService;
        _priceConfig = priceConfig;

        Price = _purchaseCount
            .Select(count => Mathf.RoundToInt(_priceConfig.basePrice * Mathf.Pow(_priceConfig.multiplier, count)))
            .ToReadOnlyReactiveProperty()
            .AddTo(_disposables);

        BuyHealthAction = () =>
        {
            var currentPrice = Price.Value;

            if (_moneyService.TrySpend(currentPrice))
            {
                _gameManager.BuyHealth(1);
                _purchaseCount.Value++;
            }
            else
            {
                Debug.Log("Недостаточно средств для покупки");
            }
        };

        ButtonColor = _moneyService.CurrentMoney
            .CombineLatest(Price, (money, price) =>
            {
                return (money < price) ? Color.gray : Color.green;
            })
            .ToReadOnlyReactiveProperty()
            .AddTo(_disposables);
    }

    public void Initialize() { }

    public void Dispose() => _disposables.Dispose();
}