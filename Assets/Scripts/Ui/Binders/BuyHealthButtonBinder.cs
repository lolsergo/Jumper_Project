using MVVM;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using System;

public sealed class BuyHealthButtonBinder : IBinder
{
    private readonly Button _button;
    private readonly Image _buttonImage;
    private readonly TMP_Text _priceText;
    private readonly Action _modelAction;
    private readonly IReadOnlyReactiveProperty<Color> _buttonColor;
    private readonly IReadOnlyReactiveProperty<int> _price;
    private readonly CompositeDisposable _disposables = new();

    // Вот этот конструктор фабрика увидит и вызовет
    public BuyHealthButtonBinder(BuyHealthButtonView view, BuyHealthButtonViewModel vm)
    {
        _button = view.Button;
        _buttonImage = view.ButtonImage;
        _priceText = view.PriceText;

        _modelAction = vm.BuyHealthAction;
        _buttonColor = vm.ButtonColor;
        _price = vm.Price;

        Debug.Log("[BinderCtor] BuyHealthButtonBinder создан через (View, VM)");
    }

    void IBinder.Bind()
    {
        Debug.Log("[Binder] Bind() вызван");

        _button.onClick.AddListener(() => _modelAction?.Invoke());

        _buttonColor
            .Subscribe(color =>
            {
                _buttonImage.color = color;
                _button.interactable = (color != Color.gray);
            })
            .AddTo(_disposables);

        _price
            .Subscribe(value =>
            {
                _priceText.text = value.ToString();
                Debug.Log($"[Binder] Установлена цена {value}");
            })
            .AddTo(_disposables);
    }

    public void Unbind()
    {
        _disposables.Clear();
        _button.onClick.RemoveAllListeners();
    }
}