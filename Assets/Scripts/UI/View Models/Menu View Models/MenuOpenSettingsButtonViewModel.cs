using UniRx;
using UnityEngine;
using Zenject;
using MVVM;
using System;

public class MenuOpenSettingsButtonViewModel
{
    [Data("MenuOpenSettingsClick")]
    public readonly Action OpenSettingsAction;

    private readonly MenuService _menuManager;
    private readonly CompositeDisposable _disposables = new();

    [Inject]
    public MenuOpenSettingsButtonViewModel(MenuService menuManager)
    {
        _menuManager = menuManager;
        OpenSettingsAction = () => {
            _menuManager.OpenSettings();
        };
    }

    public void Initialize() => Debug.Log("[VM] Initialized");
    public void Dispose() => _disposables.Dispose();
}
