using UniRx;
using UnityEngine;
using Zenject;
using MVVM;
using System;

public class OpenCreationPanelButtonViewModel
{
    [Data("OpenCreationPanelButton")]
    public readonly Action OpenCreationPanelAction;

    private readonly ProfilesSceneController _profilesSceneController;
    private readonly CompositeDisposable _disposables = new();

    [Inject]
    public OpenCreationPanelButtonViewModel(ProfilesSceneController profilesSceneController)
    {
        _profilesSceneController = profilesSceneController;
        OpenCreationPanelAction = () => {
            _profilesSceneController.OpenCreateProfilePanel();
        };
    }

    public void Initialize() => Debug.Log("[VM] Initialized");
    public void Dispose() => _disposables.Dispose();
}
