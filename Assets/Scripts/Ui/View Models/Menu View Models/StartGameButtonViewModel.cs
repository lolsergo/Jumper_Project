using UniRx;
using Zenject;
using UnityEngine;
using System;
using MVVM;

public sealed class StartGameButtonViewModel : IInitializable, IDisposable
{
    [Data("StartGameClick")]
    public readonly Action PauseAction;

    private readonly MenuManager _menuManager;
    private readonly CompositeDisposable _disposables = new();

    [Inject]
    public StartGameButtonViewModel(MenuManager menuManager)
    {
        _menuManager = menuManager;
        PauseAction = () => {
            _menuManager.StartGame();
        };
    }

    public void Initialize() { }
    public void Dispose() => _disposables.Dispose();
}

