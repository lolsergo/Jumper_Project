using MVVM;
using UnityEngine;
using Zenject;

public class BootstrapInstaller : MonoInstaller
{
    [SerializeField] private AudioCoreInstaller _audioInstaller;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<AudioServiceInitializer>().AsSingle().NonLazy();
        BinderFactory.RegisterBinder<ButtonBinder>();
    }
}
