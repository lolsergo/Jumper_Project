using MVVM;
using System;
using UniRx;
using UnityEngine.UI;

public sealed class ButtonInteractableBinder : IBinder, IObserver<bool>
{
    private readonly Button _button;
    private readonly IReadOnlyReactiveProperty<bool> _property;
    private IDisposable _subscription;

    public ButtonInteractableBinder(Button button, IReadOnlyReactiveProperty<bool> property)
    {
        _button = button;
        _property = property;
    }

    public void Bind()
    {
        _subscription = _property.Subscribe(this);
    }

    public void Unbind()
    {
        _subscription?.Dispose();
    }

    public void OnNext(bool value) => _button.interactable = value;
    public void OnCompleted() { }
    public void OnError(Exception error) { }
}
