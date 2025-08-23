using MVVM;
using UnityEngine;
using Zenject;

public sealed class ProfileSceneInstaller : MonoInstaller
{
    [SerializeField] private ProfilesListView _profilesListView;
    [SerializeField] private ProfileButtonView _buttonPrefab;

    public override void InstallBindings()
    {
        // ВАЖНО: до регистрации биндеров
        BinderFactory.SetDiContainer(Container);

        ValidateSerialized();
        BindLogic();
        BindControllers();
        BindViews();
        RegisterBinders();
        BindInitializationPipeline();
    }

    private void ValidateSerialized()
    {
        Container.AssertNotNull(_profilesListView, nameof(_profilesListView), nameof(ProfileSceneInstaller));
        Container.AssertNotNull(_buttonPrefab, nameof(_buttonPrefab), nameof(ProfileSceneInstaller));
    }

    private void BindLogic()
    {
        Container.Bind<IProfilesSceneLogic>().To<ProfilesSceneLogic>().AsSingle();
    }

    private void BindControllers()
    {
        Container.Bind<ProfilesSceneUIController>()
                 .FromComponentInHierarchy()
                 .AsSingle();
    }

    private void BindViews()
    {
        // Binding prefab instance (можно удалить если нигде не Resolve<ProfileButtonView>())
        Container.Bind<ProfileButtonView>().FromInstance(_buttonPrefab).AsSingle();
        Container.Bind<ProfilesListView>().FromInstance(_profilesListView).AsSingle();
    }

    private void RegisterBinders()
    {
        BinderFactory.RegisterBinder<ProfilesContainerBinder>();
    }

    private void BindInitializationPipeline()
    {
        Container.BindInterfacesAndSelfTo<ProfilesInitializer>()
                 .AsSingle()
                 .NonLazy();
    }
}