using Zenject;

public class ViewModelsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        BindDisplaysViewModels();
        BindButtonsViewModels();
    }

    private void BindButtonsViewModels()
    {
        Container.BindViewModel<PauseButtonViewModel>();
        Container.BindViewModel<ResumeButtonViewModel>();
        Container.BindViewModel<MenuButtonViewModel>();
        Container.BindViewModel<GameOpenSettingsButtonViewModel>();
        Container.BindViewModel<GameCloseSettingsButtonViewModel>();

        Container.BindViewModel<ReviveButtonViewModel>();
        Container.BindViewModel<SurrenderButtonViewModel>();
        Container.BindViewModel<BuyHealthButtonViewModel>();
    }

    private void BindDisplaysViewModels()
    {
        Container.BindViewModel<CurrencyViewModel>();
        Container.BindViewModel<HeartsViewModel>();
        Container.BindViewModel<DisplayDistanceViewModel>();
        Container.BindViewModel<MaxDistanceViewModel>();
        Container.BindViewModel<GameStatsViewModel>();
    }
}
