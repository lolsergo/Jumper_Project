using UniRx;
using UnityEngine;
using Zenject;
using MVVM;
using System;

public class OpenSettingsButtonViewModel
{
    [Data("OpenSettingsClick")]
    public readonly Action OpenSettingsAction;

    private readonly MenuManager _menuManager;
    private readonly CompositeDisposable _disposables = new();

    [Inject]
    public OpenSettingsButtonViewModel(MenuManager menuManager)
    {
        _menuManager = menuManager;
        OpenSettingsAction = () => {
            _menuManager.OpenSettings();
        };
    }

    public void Initialize() => Debug.Log("[VM] Initialized");
    public void Dispose() => _disposables.Dispose();
}
