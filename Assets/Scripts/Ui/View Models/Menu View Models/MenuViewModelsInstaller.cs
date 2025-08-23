using Zenject;

public class MenuViewModelsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindViewModel<StartGameButtonViewModel>();
        Container.BindViewModel<QuitButtonViewModel>();        
        Container.BindViewModel<MenuOpenSettingsButtonViewModel>();
        Container.BindViewModel<MenuCloseSettingsButtonViewModel>();        
        Container.BindViewModel<ChangeProfileButtonViewModel>();        
    }
}
