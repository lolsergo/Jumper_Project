using MVVM;
using System;
using Zenject;
using UnityEngine;

public sealed class BindersInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        BinderFactory.RegisterBinder<TextBinder>();
        BinderFactory.RegisterBinder<ImageBinder>();
        BinderFactory.RegisterBinder<BuyHealthButtonBinder>();
        BinderFactory.RegisterBinder<ImageColorBinder>();
        BinderFactory.RegisterBinder<ButtonBinder>();
    }
}
