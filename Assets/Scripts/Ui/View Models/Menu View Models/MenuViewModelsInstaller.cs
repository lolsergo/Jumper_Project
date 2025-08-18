using Zenject;

public class MenuViewModelsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        BindAsSingle<StartGameButtonViewModel>();
        BindAsSingle<QuitButtonViewModel>();
    }

    private void BindAsSingle<T>() where T : class
    {
        this.Container
            .BindInterfacesAndSelfTo<T>()
            .AsSingle()
            .NonLazy();
    }
}
