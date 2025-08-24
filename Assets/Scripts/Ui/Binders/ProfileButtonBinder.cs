using MVVM;
using System;
using UniRx;
using UnityEngine.UI;
using TMPro;

public sealed class ProfileButtonBinder : IBinder
{
    private readonly TMP_Text _label;
    private readonly Button _button;
    private readonly ProfileButtonViewModel _viewModel;

    private IDisposable _labelSubscription;

    public ProfileButtonBinder(TMP_Text label, Button button, ProfileButtonViewModel viewModel)
    {
        _label = label;
        _button = button;
        _viewModel = viewModel;
    }

    public void Bind()
    {
        // подписка на изменение текста
        _labelSubscription = _viewModel.Label.Subscribe(value => _label.text = value);

        // связываем клик с ReactiveCommand
        _button.onClick.AddListener(() => _viewModel.OnClick.Execute());
    }

    public void Unbind()
    {
        _labelSubscription?.Dispose();
        _button.onClick.RemoveAllListeners();
    }
}
