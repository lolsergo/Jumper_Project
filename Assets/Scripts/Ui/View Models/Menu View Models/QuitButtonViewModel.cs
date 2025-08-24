using UniRx;
using Zenject;
using System;
using MVVM;

public sealed class QuitButtonViewModel : IInitializable, IDisposable
{
    [Data("QuitClick")]
    public readonly Action PauseAction;

    private readonly MenuService _menuManager;
    private readonly CompositeDisposable _disposables = new();

    [Inject]
    public QuitButtonViewModel(MenuService menuManager)
    {
        _menuManager = menuManager;
        PauseAction = () => {
            _menuManager.Quit();
        };
    }

    public void Initialize() { }
    public void Dispose() => _disposables.Dispose();
}

