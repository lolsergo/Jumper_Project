using Zenject;

public class ProfilePanelViewModelsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindViewModel<CreateProfileAcceptButtonViewModel>();
        Container.BindViewModel<NewProfileNameInputViewModel>();
        Container.BindViewModel<OpenCreationPanelButtonViewModel>();
        Container.BindViewModel<ProfilesListViewModel>();
        Container.BindViewModel<AcceptDeleteProfileButtonViewModel>();
        Container.BindViewModel<CancelDeleteButtonViewModel>();
    }
}
