using UnityEngine;
using Zenject;

public class MenuInstaller : MonoInstaller
{
    [SerializeField]
    private MenuManager _menuManager;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<MenuManager>()
            .FromInstance(_menuManager)
            .AsSingle();
    }
}
