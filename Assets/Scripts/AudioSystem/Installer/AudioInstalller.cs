using MVVM;
using UnityEngine;
using Zenject;

[CreateAssetMenu(menuName = "Audio/AudioInstaller")]
public class AudioCoreInstaller : ScriptableObjectInstaller
{
    [SerializeField] private AudioLibrary _library;
    [SerializeField] private AudioSource _sourcePrefab;

    public override void InstallBindings()
    {
        // Основные биндинги аудио системы
        Container.BindInstance(_library).AsSingle();
        Container.Bind<AudioSource>().FromMethod(CreateAudioSource).AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<AudioPoolRegistry>().AsSingle();
        Container.Bind<AudioManager>().AsSingle().NonLazy();

        // Биндинги для кнопок
        Container.Bind<AudioButtonViewModel>().AsTransient();

        // Правильная регистрация фабрики для AudioBinder
        Container.Bind<IBinder>()
            .WithId("AudioBinder")
            .FromMethod(ctx => {
                var view = ctx.ObjectInstance as AudioButtonView;
                var vm = ctx.Container.Resolve<AudioButtonViewModel>();
                return new AudioBinder(view.Button, view.SoundID, vm.OnSoundRequested);
            })
            .WhenInjectedInto<MonoViewBinder>();
    }

    private AudioSource CreateAudioSource(InjectContext ctx)
    {
        return _sourcePrefab != null
            ? Container.InstantiatePrefabForComponent<AudioSource>(_sourcePrefab)
            : CreateDefaultSource();
    }

    private AudioSource CreateDefaultSource()
    {
        var go = new GameObject("DefaultAudioSource");
        DontDestroyOnLoad(go);
        return go.AddComponent<AudioSource>();
    }
}