using UniRx;
using UnityEngine;
using System;
using Zenject;
using MVVM;


public class GameOpenSettingsButtonViewModel
{
    [Data("GameOpenSettingsClick")]
    public readonly Action OpenSettingsAction;

    private readonly UIGameServise _gameManager;
    private readonly CompositeDisposable _disposables = new();

    [Inject]
    public GameOpenSettingsButtonViewModel(UIGameServise gameManager)
    {
        _gameManager = gameManager;
        OpenSettingsAction = () => {
            _gameManager.ShowSettings();
        };
    }

    public void Initialize() => Debug.Log("[VM] Initialized");
    public void Dispose() => _disposables.Dispose();
}
