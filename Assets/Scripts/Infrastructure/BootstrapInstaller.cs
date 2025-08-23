using MVVM;
using UnityEngine;
using Zenject;

public class BootstrapInstaller : MonoInstaller
{
    [SerializeField] private AudioCoreInstaller _audioInstaller;
    [SerializeField] private SceneInjectionConfig _sceneInjectionConfig;
    [SerializeField] private InputController _inputControllerPrefab;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<AudioServiceInitializer>().AsSingle().NonLazy();

        Container.BindInstance(_sceneInjectionConfig).AsSingle();
        Container.BindInterfacesAndSelfTo<SceneInjectionHandler>().AsSingle();
        Container.Bind<PlayerProvider>().AsSingle();

        // Создаём singleton InputController. ProjectContext уже помечен DontDestroyOnLoad,
        // поэтому отдельный вызов не нужен.
        Container.Bind<InputController>()
            .FromComponentInNewPrefab(_inputControllerPrefab)
            .AsSingle()
            .NonLazy();

        Container.Bind<IUserProfileService>().To<UserProfileService>().AsSingle();
        Container.Bind<ISettingsService>().To<SettingsService>().AsSingle();
        Container.BindViewModel<AudioSettingsViewModel>();
        Container.BindViewModel<ResolutionDropdownViewModel>();

#if UNITY_EDITOR
        const string rewardedId = "ca-app-pub-3940256099942544/5224354917";
#else
        const string rewardedId = "ca-app-pub-3940256099942544/5224354917";
#endif
        AdsInstaller.Install(Container, rewardedId);
    }
}