using UniRx;
using Zenject;
using UnityEngine;
using System;
using MVVM;

public class BuyHealthButtonViewModel : IInitializable, IDisposable
{
    [Data("BuyHealthClick")]
    public readonly Action BuyHealthAction;

    [Data("ButtonColor")]
    public readonly IReadOnlyReactiveProperty<Color> ButtonColor; // Возвращаем оригинальный тип

    private readonly GameManager _gameManager;
    private readonly CompositeDisposable _disposables = new();
    private readonly BoolReactiveProperty _hasPurchased = new(false);

    [Inject]
    public BuyHealthButtonViewModel(GameManager gameManager)
    {
        _gameManager = gameManager;

        BuyHealthAction = () =>
        {
            if (!_hasPurchased.Value)
            {
                _gameManager.BuyHealth();
                _hasPurchased.Value = true;
            }
        };

        ButtonColor = _hasPurchased
            .Select(hasPurchased => hasPurchased ? Color.gray : Color.green)
            .ToReadOnlyReactiveProperty();
    }

    public void Initialize() => Debug.Log("[VM] Initialized");
    public void Dispose() => _disposables.Dispose();
}
