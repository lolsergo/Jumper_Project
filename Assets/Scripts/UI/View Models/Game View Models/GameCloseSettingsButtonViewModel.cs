using UniRx;
using UnityEngine;
using System;
using Zenject;
using MVVM;

public class GameCloseSettingsButtonViewModel
{
    [Data("GameCloseSettingsClick")]
    public readonly Action OpenSettingsAction;

    private readonly UIGameManager _gameManager;
    private readonly CompositeDisposable _disposables = new();

    [Inject]
    public GameCloseSettingsButtonViewModel(UIGameManager gameManager)
    {
        _gameManager = gameManager;
        OpenSettingsAction = () => {
            _gameManager.HideSettings();
        };
    }

    public void Initialize() => Debug.Log("[VM] Initialized");
    public void Dispose() => _disposables.Dispose();
}
