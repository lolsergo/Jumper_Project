using UnityEngine;
using Zenject;

[CreateAssetMenu(menuName = "Audio/AudioInstaller")]
public class AudioCoreInstaller : ScriptableObjectInstaller<AudioCoreInstaller>
{
    [SerializeField] private AudioLibrary _library;
    [SerializeField] private GameObject _sourcePrefab;

    [System.Serializable]
    public class Settings
    {
        public GameObject SourcePrefab;
    }

    public override void InstallBindings()
    {
        if (_library == null)
            throw new System.NullReferenceException("AudioLibrary not assigned in AudioCoreInstaller!");

        Container.BindInstance(_library).AsSingle();

        var settings = new Settings { SourcePrefab = _sourcePrefab };
        Container.BindInstance(settings).AsSingle();

        Container.BindInterfacesAndSelfTo<AudioPoolRegistry>().AsSingle();
        Container.Bind<AudioProvider>().AsSingle().NonLazy();
        Container.Bind<GameAudioSettings>().AsSingle();
    }
}