using MVVM;
using System;
using TMPro;
using UniRx;

public sealed class TextBinder : IBinder, IObserver<string>
{
    private readonly TMP_Text _text;
    private readonly IReadOnlyReactiveProperty<string> _property;
    private IDisposable _subscription;

    public TextBinder(TMP_Text text, IReadOnlyReactiveProperty<string> property)
    {
        _text = text;
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

    // Реализация IObserver<string>
    public void OnNext(string value) => _text.text = value;
    public void OnCompleted() { }
    public void OnError(Exception error) { }
}
