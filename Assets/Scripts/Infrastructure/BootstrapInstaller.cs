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
        BinderFactory.RegisterBinder<ButtonBinder>();

        Container.BindInstance(_sceneInjectionConfig).AsSingle();
        Container.BindInterfacesAndSelfTo<SceneInjectionHandler>().AsSingle();
        Container.Bind<PlayerProvider>().AsSingle();

        var input = Container.InstantiatePrefabForComponent<InputController>(_inputControllerPrefab);
        Object.DontDestroyOnLoad(input.gameObject);
        Container.Bind<InputController>().FromInstance(input).AsSingle();
    }
}