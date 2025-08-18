using MVVM;
using System;
using Zenject;
using UnityEngine;

public sealed class MenuBinderInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        BinderFactory.RegisterBinder<ButtonBinder>();
    }
}
