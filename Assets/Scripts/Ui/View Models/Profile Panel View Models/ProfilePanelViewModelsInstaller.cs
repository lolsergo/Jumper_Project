using UnityEngine;
using Zenject;

public class ProfilePanelViewModelsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        BindAsSingle<CreateProfileAcceptButtonViewModel>();
        BindAsSingle<NewProfileNameInputViewModel>();
        BindAsSingle<OpenCreationPanelButtonViewModel>();

    }

    private void BindAsSingle<T>() where T : class
    {
        this.Container
            .BindInterfacesAndSelfTo<T>()
            .AsSingle()
            .NonLazy();
    }
}
