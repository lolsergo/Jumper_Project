using Zenject;

public class InputRebindInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        // Bind service (IInitializable will call Initialize automatically if NonLazy).
        Container.BindInterfacesAndSelfTo<InputRebindService>()
            .AsSingle()
            .NonLazy();

        Container.BindFactory<InputController.InputActionType, int, RebindActionButtonViewModel,
            RebindActionButtonViewModel.Factory>();

        // Explicit early initialization to ensure bindings are cached before UI setups.
        var svc = Container.Resolve<IInputRebindService>();
        svc.Initialize();
    }
}