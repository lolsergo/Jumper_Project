using Zenject;

public class ViewModelsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        BindAsSingle<CurrencyViewModel>();
        BindAsSingle<HeartsViewModel>();
        BindAsSingle<PauseButtonViewModel>();
        BindAsSingle<ResumeButtonViewModel>();
        BindAsSingle<MenuButtonViewModel>();
        BindAsSingle<BuyHealthButtonViewModel>();
    }

    private void BindAsSingle<T>() where T : class
    {
        this.Container
            .BindInterfacesAndSelfTo<T>()
            .AsSingle()
            .NonLazy();
    }
}
