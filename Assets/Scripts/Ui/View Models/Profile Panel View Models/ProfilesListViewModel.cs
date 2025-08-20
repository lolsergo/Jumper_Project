// ViewModel для списка профилей
using UniRx;
using MVVM;
using Zenject;
using System;

public class ProfilesListViewModel : IDisposable
{
    [Data("ProfilesContainer")]
    public readonly ReactiveCollection<ProfileButtonViewModel> Profiles = new();

    [Data("CreateProfile")]
    public readonly ReactiveCommand CreateProfile = new();

    private readonly IUserProfileService _profileService;
    private readonly CompositeDisposable _disposables = new();

    [Inject]
    public ProfilesListViewModel(IUserProfileService profileService)
    {
        _profileService = profileService;

        // Кнопка "Создать"
        CreateProfile.Subscribe(_ =>
        {
            var name = $"Profile_{UnityEngine.Random.Range(1, 9999)}";
            _profileService.CreateProfile(name);
        }).AddTo(_disposables);

        // Следим за изменениями списка в сервисе
        _profileService.Profiles
            .ObserveAdd()
            .Subscribe(x => AddButtonVM(x.Value))
            .AddTo(_disposables);

        _profileService.Profiles
            .ObserveRemove()
            .Subscribe(_ => RebuildAll())
            .AddTo(_disposables);

        _profileService.Profiles
            .ObserveReset()
            .Subscribe(_ => RebuildAll())
            .AddTo(_disposables);

        // Первичная отрисовка
        RebuildAll();
    }

    private void AddButtonVM(string profileName)
    {
        var vm = new ProfileButtonViewModel(profileName);
        vm.OnClick
          .Subscribe(_ => _profileService.LoadProfile(profileName))
          .AddTo(_disposables);

        Profiles.Add(vm);
    }

    private void RebuildAll()
    {
        Profiles.Clear();
        foreach (var name in _profileService.Profiles)
            AddButtonVM(name);
    }

    public void Dispose() => _disposables.Dispose();
}
