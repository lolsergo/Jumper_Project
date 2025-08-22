using UniRx;
using UnityEngine;
using System;
using Zenject;
using MVVM;

public class CancelDeleteButtonViewModel
{
    [Data("CancelDeleteClick")]
    public readonly Action CloseCreationPanelAction;

    private readonly ProfilesSceneUIController _profilesSceneUIController;
    private readonly CompositeDisposable _disposables = new();

    [Inject]
    public CancelDeleteButtonViewModel(ProfilesSceneUIController profilesSceneController)
    {
        _profilesSceneUIController = profilesSceneController;
        CloseCreationPanelAction = () => {
            _profilesSceneUIController.CloseAcceptDeleteProfilePanel();
        };
    }

    public void Initialize() => Debug.Log("[VM] Initialized");
    public void Dispose() => _disposables.Dispose();
}
