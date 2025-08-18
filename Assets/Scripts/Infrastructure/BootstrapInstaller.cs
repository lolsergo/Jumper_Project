using UnityEngine;
using Zenject;
using MVVM;

public class BootstrapInstaller : MonoInstaller
{
    [SerializeField] private AudioCoreInstaller _audioInstaller;

    public override void InstallBindings()
    {
        // 1. Регистрация системы биндеров (как у вас было)
        BinderFactory.RegisterBinder<ButtonBinder>();
        BinderFactory.RegisterBinder<AudioBinder>();

        // 2. Установка аудиосистемы
        Container.BindInstance(_audioInstaller).AsSingle();
        _audioInstaller.InstallBindings();

        // 3. Регистрация ViewModel (без явного резолва зависимостей)
        Container.Bind<AudioButtonViewModel>().AsTransient();
    }
}
