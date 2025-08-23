using UniRx;
using Zenject;
using UnityEngine;
using System;
using MVVM;

public sealed class QuitButtonViewModel : IInitializable, IDisposable
{
    [Data("QuitClick")]
    public readonly Action PauseAction;

    private readonly MenuManager _menuManager;
    private readonly CompositeDisposable _disposables = new();

    [Inject]
    public QuitButtonViewModel(MenuManager menuManager)
    {
        _menuManager = menuManager;
        PauseAction = () => {
            _menuManager.Quit();
        };
    }

    public void Initialize() { }
    public void Dispose() => _disposables.Dispose();
}

