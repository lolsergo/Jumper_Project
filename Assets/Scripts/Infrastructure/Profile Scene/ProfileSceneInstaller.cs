using MVVM;
using UnityEngine;
using Zenject;

public sealed class ProfileSceneInstaller : MonoInstaller
{
    [SerializeField] private ProfilesListView _profilesListView;
    [SerializeField] private ProfileButtonView _buttonPrefab;

    public override void InstallBindings()
    {
        // 1. ���������� �����
        Container.Bind<ProfilesSceneController>()
                 .FromComponentInHierarchy()
                 .AsSingle();

        // 2. MVVM: prefab ������
        Container.Bind<ProfileButtonView>()
                 .FromInstance(_buttonPrefab)
                 .AsSingle();

        // 3. View ������
        Container.Bind<ProfilesListView>()
                 .FromInstance(_profilesListView)
                 .AsSingle();

        // 4. ViewModel ������
        Container.Bind<ProfilesListViewModel>()
                 .AsSingle();

        BinderFactory.RegisterBinder<ProfilesContainerBinder>();
        // 5. ������������� ���������
        Container.BindInterfacesAndSelfTo<ProfilesInitializer>()
                 .AsSingle()
                 .NonLazy();

    }
}