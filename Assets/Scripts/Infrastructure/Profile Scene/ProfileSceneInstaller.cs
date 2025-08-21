using MVVM;
using UnityEngine;
using Zenject;

public sealed class ProfileSceneInstaller : MonoInstaller
{
    [SerializeField] private ProfilesListView _profilesListView;
    [SerializeField] private ProfileButtonView _buttonPrefab;

    public override void InstallBindings()
    {
        // 1. Контроллер сцены
        Container.Bind<ProfilesSceneController>()
                 .FromComponentInHierarchy()
                 .AsSingle();

        // 2. MVVM: prefab кнопки
        Container.Bind<ProfileButtonView>()
                 .FromInstance(_buttonPrefab)
                 .AsSingle();

        // 3. View списка
        Container.Bind<ProfilesListView>()
                 .FromInstance(_profilesListView)
                 .AsSingle();

        BinderFactory.RegisterBinder<ProfilesContainerBinder>();
        // 5. Инициализация биндингов
        Container.BindInterfacesAndSelfTo<ProfilesInitializer>()
                 .AsSingle()
                 .NonLazy();

    }
}