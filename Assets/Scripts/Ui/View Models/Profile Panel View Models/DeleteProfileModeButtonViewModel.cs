using UniRx;
using MVVM;
using System;

public class DeleteProfileModeButtonViewModel : IDisposable
{
    [Data("ClickCommand")]
    public readonly ReactiveCommand ClickCommand;

    public readonly ReactiveProperty<bool> IsDeleteMode;

    public DeleteProfileModeButtonViewModel(
        ReactiveProperty<bool> isDeleteMode,
        ReactiveCommand toggleDeleteMode)
    {
        IsDeleteMode = isDeleteMode;
        ClickCommand = toggleDeleteMode;
    }

    public void Dispose() { }
}
