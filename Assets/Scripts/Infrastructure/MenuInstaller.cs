using UnityEngine;
using Zenject;

public class MenuInstaller : MonoInstaller
{
    [SerializeField]
    private MenuService _menuManager;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<MenuService>()
            .FromInstance(_menuManager)
            .AsSingle();
    }
}
