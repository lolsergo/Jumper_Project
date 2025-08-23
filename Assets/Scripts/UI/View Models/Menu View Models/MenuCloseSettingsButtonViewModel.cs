using UniRx;
using UnityEngine;
using Zenject;
using MVVM;
using System;

public class MenuCloseSettingsButtonViewModel
{
    [Data("MenuCloseSettingsClick")]
    public readonly Action OpenSettingsAction;

    private readonly MenuManager _menuManager;
    private readonly CompositeDisposable _disposables = new();

    [Inject]
    public MenuCloseSettingsButtonViewModel(MenuManager menuManager)
    {
        _menuManager = menuManager;
        OpenSettingsAction = () => {
            _menuManager.CloseSettings();
        };
    }

    public void Initialize() => Debug.Log("[VM] Initialized");
    public void Dispose() => _disposables.Dispose();
}
