using MVVM;
using System;
using TMPro;
using UniRx;

public sealed class InputFieldBinder : IBinder, IObserver<string>
{
    private readonly TMP_InputField _inputField;
    private readonly ReactiveProperty<string> _property;

    private IDisposable _subscription;
    private bool _updatingFromView;
    private bool _updatingFromVM;

    public InputFieldBinder(TMP_InputField inputField, ReactiveProperty<string> property)
    {
        _inputField = inputField;
        _property = property;
    }

    public void Bind()
    {
        _subscription = _property.Subscribe(this);
        _inputField.onValueChanged.AddListener(OnViewChanged);
    }

    public void Unbind()
    {
        _subscription?.Dispose();
        _inputField.onValueChanged.RemoveListener(OnViewChanged);
    }

    public void OnNext(string value)
    {
        if (_updatingFromView) return;
        _updatingFromVM = true;
        _inputField.text = value ?? string.Empty;
        _updatingFromVM = false;
    }

    private void OnViewChanged(string newValue)
    {
        if (_updatingFromVM) return;
        _updatingFromView = true;
        _property.Value = newValue;
        _updatingFromView = false;
    }

    public void OnCompleted() { }
    public void OnError(Exception error) { }
}