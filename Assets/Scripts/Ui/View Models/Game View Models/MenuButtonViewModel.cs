using UniRx;
using Zenject;
using UnityEngine;
using System;
using MVVM;

public sealed class MenuButtonViewModel : IInitializable, IDisposable
{
    [Data("MainMenuButton")]
    public readonly Action Menu;

    private readonly GameManager _gameManager;
    private readonly CompositeDisposable _disposables = new();

    [Inject]
    public MenuButtonViewModel(GameManager gameManager)
    {
        _gameManager = gameManager;
        Menu = () => _gameManager.ReturnToMainMenu();
    }

    public void Initialize() { }
    public void Dispose() => _disposables.Dispose();
}

