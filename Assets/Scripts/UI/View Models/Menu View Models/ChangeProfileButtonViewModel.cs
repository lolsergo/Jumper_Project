using UnityEngine;
using MVVM;
using System;
using UniRx;
using Zenject;

public class ChangeProfileButtonViewModel
{
    [Data("ChangeProfileClick")]
    public readonly Action ChangeProfileAction;

    private readonly MenuService _menuManager;
    private readonly CompositeDisposable _disposables = new();

    [Inject]
    public ChangeProfileButtonViewModel(MenuService menuManager)
    {
        _menuManager = menuManager;
        ChangeProfileAction = () => {
            _menuManager.ChangeProfile();
        };
    }

    public void Initialize() => Debug.Log("[VM] Initialized");
    public void Dispose() => _disposables.Dispose();
}
