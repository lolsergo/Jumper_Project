using Zenject;

public class ViewModelsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindViewModel<CurrencyViewModel>();
        Container.BindViewModel<HeartsViewModel>();
        Container.BindViewModel<PauseButtonViewModel>();
        Container.BindViewModel<ResumeButtonViewModel>();
        Container.BindViewModel<MenuButtonViewModel>();
        Container.BindViewModel<BuyHealthButtonViewModel>();
        Container.BindViewModel<DisplayDistanceViewModel>();
        Container.BindViewModel<MaxDistanceViewModel>();
        Container.BindViewModel<SurrenderButtonViewModel>();
        Container.BindViewModel<GameStatsViewModel>();
        Container.BindViewModel<ReviveButtonViewModel>();
    }
}
