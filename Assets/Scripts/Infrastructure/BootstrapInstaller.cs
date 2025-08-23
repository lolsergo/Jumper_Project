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

        var input = Container.InstantiatePrefabForComponent<InputController>(_inputControllerPrefab);
        Object.DontDestroyOnLoad(input.gameObject);
        Container.Bind<InputController>().FromInstance(input).AsSingle();

        Container.Bind<IUserProfileService>().To<UserProfileService>().AsSingle();
        Container.Bind<ISettingsService>().To<SettingsService>().AsSingle();
        Container.BindViewModel<AudioSettingsViewModel>();
        Container.BindViewModel<ResolutionDropdownViewModel>();

        // === Ads ===
#if UNITY_EDITOR
        const string rewardedId = "ca-app-pub-3940256099942544/5224354917"; // тест
#else
        const string rewardedId = "ca-app-pub-3940256099942544/5224354917"; // для учебы можно оставить тест
#endif
        AdsInstaller.Install(Container, rewardedId);
    }
}