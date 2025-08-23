using UnityEngine;
using MVVM;
using System;
using UniRx;
using Zenject;

public class ChangeProfileButtonViewModel
{
    [Data("ChangeProfileClick")]
    public readonly Action ChangeProfileAction;

    private readonly MenuManager _menuManager;
    private readonly CompositeDisposable _disposables = new();

    [Inject]
    public ChangeProfileButtonViewModel(MenuManager menuManager)
    {
        _menuManager = menuManager;
        ChangeProfileAction = () => {
            _menuManager.ChangeProfile();
        };
    }

    public void Initialize() => Debug.Log("[VM] Initialized");
    public void Dispose() => _disposables.Dispose();
}
