using MVVM;
using System;
using UniRx;
using UnityEngine.SceneManagement;
using Zenject;
using System.Linq;

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
    private readonly ProfilesSceneUIController _sceneController;
    private readonly CompositeDisposable _disposables = new();

    [Inject]
    public ProfilesListViewModel(IUserProfileService profileService,
                                 ProfilesSceneUIController sceneController)
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
            .Subscribe(x => RemoveButtonVM(x.Value))
            .AddTo(_disposables);

        _profileService.Profiles.ObserveReset()
            .Subscribe(_ => RebuildAll())
            .AddTo(_disposables);

        RebuildAll();
    }

    private void AddButtonVM(string profileName)
    {
        var vm = new ProfileButtonViewModel(profileName);

        var clickDisp = vm.OnClick
            .Subscribe(_ =>
            {
                if (IsDeleteMode.Value)
                {
                    _sceneController.OpenAcceptDeleteProfilePanel(profileName);
                }
                else
                {
                    _profileService.LoadProfile(profileName);
                    SceneLoader.Load(SceneType.Menu);
                }
            });
        vm.AddDisposable(clickDisp);

        var modeDisp = IsDeleteMode
            .Subscribe(val => vm.IsDeleteMode.Value = val);
        vm.AddDisposable(modeDisp);

        Profiles.Add(vm);
    }

    private void RemoveButtonVM(string profileName)
    {
        var vm = Profiles.FirstOrDefault(p => p.Label.Value == profileName);
        if (vm != null)
        {
            Profiles.Remove(vm);
            vm.Dispose();
        }
    }

    private void RebuildAll()
    {
        foreach (var vm in Profiles)
            vm.Dispose();
        Profiles.Clear();
        foreach (var name in _profileService.Profiles)
            AddButtonVM(name);
    }

    public void Dispose()
    {
        DeleteModeButtonVM.Dispose();
        foreach (var vm in Profiles)
            vm.Dispose();
        _disposables.Dispose();
    }
}