using UniRx;
using Zenject;
using System;
using MVVM;

public sealed class StartGameButtonViewModel : IInitializable, IDisposable
{
    [Data("StartGameClick")]
    public readonly Action PauseAction;

    private readonly MenuService _menuManager;
    private readonly CompositeDisposable _disposables = new();

    [Inject]
    public StartGameButtonViewModel(MenuService menuManager)
    {
        _menuManager = menuManager;
        PauseAction = () => {
            _menuManager.StartGame();
        };
    }

    public void Initialize() { }
    public void Dispose() => _disposables.Dispose();
}

