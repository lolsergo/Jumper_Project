using UniRx;
using MVVM;
using System;

public class ProfileButtonViewModel : IDisposable
{
    [Data("Label")]
    public readonly ReactiveProperty<string> Label = new();

    [Data("OnClick")]
    public readonly ReactiveCommand OnClick = new();

    [Data("IsDeleteMode")]
    public readonly ReactiveProperty<bool> IsDeleteMode = new(false);

    private readonly CompositeDisposable _disposables = new();

    public ProfileButtonViewModel(string label)
    {
        Label.Value = label;
    }

    public void AddDisposable(IDisposable disposable)
    {
        _disposables.Add(disposable);
    }

    public void Dispose()
    {
        _disposables.Dispose();
        Label.Dispose();
        OnClick.Dispose();
        IsDeleteMode.Dispose();
    }
}
