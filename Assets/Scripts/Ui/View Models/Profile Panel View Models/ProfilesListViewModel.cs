using MVVM;
using System;
using UniRx;
using UnityEngine.SceneManagement;
using Zenject;

public class ProfilesListViewModel : IDisposable
{
    [Data("ProfilesContainer")]
    public readonly ReactiveCollection<ProfileButtonViewModel> Profiles = new();

    [Data("CreateProfile")]
    public readonly ReactiveCommand CreateProfile = new();

    [Data("ToggleDeleteMode")]
    public readonly ReactiveCommand ToggleDeleteMode = new();

    [Data("IsDeleteMode")]
    public readonly ReactiveProperty<bool> IsDeleteMode = new(false);

    [Data("DeleteModeButton")]
    public readonly DeleteProfileModeButtonViewModel DeleteModeButtonVM;

    private readonly IUserProfileService _profileService;
    private readonly ProfilesSceneController _sceneController;
    private readonly CompositeDisposable _disposables = new();

    [Inject]
    public ProfilesListViewModel(IUserProfileService profileService,
                                 ProfilesSceneController sceneController)
    {
        _profileService = profileService;
        _sceneController = sceneController;

        DeleteModeButtonVM = new DeleteProfileModeButtonViewModel(
            IsDeleteMode,
            ToggleDeleteMode
        );

        ToggleDeleteMode
            .Subscribe(_ => IsDeleteMode.Value = !IsDeleteMode.Value)
            .AddTo(_disposables);

        _profileService.Profiles.ObserveAdd()
            .Subscribe(x => AddButtonVM(x.Value))
            .AddTo(_disposables);

        _profileService.Profiles.ObserveRemove()
            .Subscribe(_ => RebuildAll())
            .AddTo(_disposables);

        _profileService.Profiles.ObserveReset()
            .Subscribe(_ => RebuildAll())
            .AddTo(_disposables);

        RebuildAll();
    }

    private void AddButtonVM(string profileName)
    {
        var vm = new ProfileButtonViewModel(profileName);

        vm.OnClick
          .Subscribe(_ =>
          {
              if (IsDeleteMode.Value)
              {
                  _sceneController.OpenAcceptDeleteProfilePanel(profileName);
              }
              else
              {
                  _profileService.LoadProfile(profileName);
                  SceneManager.LoadScene("Main Menu");
              }
          })
          .AddTo(_disposables);

        IsDeleteMode
            .Subscribe(val => vm.IsDeleteMode.Value = val)
            .AddTo(_disposables);

        Profiles.Add(vm);
    }

    private void RebuildAll()
    {
        Profiles.Clear();
        foreach (var name in _profileService.Profiles)
            AddButtonVM(name);
    }

    public void Dispose()
    {
        DeleteModeButtonVM.Dispose();
        _disposables.Dispose();
    }
}