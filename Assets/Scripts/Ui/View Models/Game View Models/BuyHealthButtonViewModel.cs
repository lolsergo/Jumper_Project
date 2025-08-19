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
    public readonly IReadOnlyReactiveProperty<int> Price; // Для привязки цены в UI

    private readonly GameManager _gameManager;
    private readonly IMoneyService _moneyService;
    private readonly PriceConfigSO _priceConfig;

    private readonly CompositeDisposable _disposables = new();

    private readonly IntReactiveProperty _purchaseCount = new(0);

    [Inject]
    public BuyHealthButtonViewModel(
        GameManager gameManager,
        IMoneyService moneyService,
        PriceConfigSO priceConfig
    )
    {
        _gameManager = gameManager;
        _moneyService = moneyService;
        _priceConfig = priceConfig;

        // Реактивная цена: basePrice * multiplier^count
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
                _purchaseCount.Value++; // увеличиваем счётчик покупок => цена растёт
            }
            else
            {
                Debug.Log("Недостаточно средств для покупки");
            }
        };

        ButtonColor = _moneyService.CurrentMoney
            .CombineLatest(Price, (money, price) =>
            {
                // Серый, если денег меньше цены
                return (money < price) ? Color.gray : Color.green;
            })
            .ToReadOnlyReactiveProperty()
            .AddTo(_disposables);
    }

    public void Initialize() => Debug.Log("[BuyHealthButtonVM] Initialized");

    public void Dispose() => _disposables.Dispose();
}