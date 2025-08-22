using Zenject;

public class MenuViewModelsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindViewModel<StartGameButtonViewModel>();
        Container.BindViewModel<QuitButtonViewModel>();
        Container.BindViewModel<AudioSettingsViewModel>();
        Container.BindViewModel<OpenSettingsButtonViewModel>();
        Container.BindViewModel<CloseSettingsViewModel>();
        Container.BindViewModel<ResolutionDropdownViewModel>();
    }
}
