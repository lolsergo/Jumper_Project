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
        BinderFactory.RegisterBinder<InputFieldBinder>();
        BinderFactory.RegisterBinder<ButtonInteractableBinder>();
        BinderFactory.RegisterBinder<ButtonReactiveCommandBinder>();
        BinderFactory.RegisterBinder<GameObjectActiveBinder>();
        BinderFactory.RegisterBinder<ProfilesCollectionBinder>();
        BinderFactory.RegisterBinder<SliderBinder>();

        // Старые dropdown биндеры больше не нужны:
        // BinderFactory.RegisterBinder<DropdownOptionsBinder>();
        // BinderFactory.RegisterBinder<DropdownSelectedIndexBinder>();
        // BinderFactory.RegisterBinder<DropdownActionBinder>();

        BinderFactory.RegisterBinder<ResolutionDropdownBinder>();
    }
}
