using UniRx;
using UnityEngine;
using Zenject;
using MVVM;
using System;

public class CloseSettingsViewModel
{
    [Data("CloseSettingsClick")]
    public readonly Action OpenSettingsAction;

    private readonly MenuManager _menuManager;
    private readonly CompositeDisposable _disposables = new();

    [Inject]
    public CloseSettingsViewModel(MenuManager menuManager)
    {
        _menuManager = menuManager;
        OpenSettingsAction = () => {
            _menuManager.CloseSettings();
        };
    }

    public void Initialize() => Debug.Log("[VM] Initialized");
    public void Dispose() => _disposables.Dispose();
}
