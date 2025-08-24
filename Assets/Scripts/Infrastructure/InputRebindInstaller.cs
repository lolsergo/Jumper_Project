using Zenject;

public class InputRebindInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        if (!Container.HasBinding<IInputRebindService>())
        {
            Container.BindInterfacesAndSelfTo<InputRebindService>()
                .AsSingle();
        }

        Container.BindFactory<InputController.InputActionType, int, RebindActionButtonViewModel,
            RebindActionButtonViewModel.Factory>();
    }
}