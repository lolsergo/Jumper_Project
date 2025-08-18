using UnityEngine;
using Zenject;

public class AudioSystemFactory : IFactory<AudioLibrary, AudioSource, Transform, AudioManager>
{
    private readonly DiContainer _container;

    public AudioSystemFactory(DiContainer container)
    {
        _container = container;
    }

    public AudioManager Create(AudioLibrary library, AudioSource sourcePrefab = null, Transform parent = null)
    {
        // 1. Создаем родительский объект если не указан
        var audioRoot = parent != null ? parent : new GameObject("AudioSystem").transform;

        // 2. Создаем AudioSource если не указан префаб
        var audioSource = sourcePrefab != null
            ? _container.InstantiatePrefabForComponent<AudioSource>(sourcePrefab, audioRoot)
            : CreateDefaultAudioSource(audioRoot);

        // 3. Регистрируем зависимости
        _container.BindInstance(library).AsSingle();
        _container.BindInstance(audioSource).AsSingle();

        // 4. Создаем и возвращаем AudioManager
        return _container.Instantiate<AudioManager>();
    }

    private AudioSource CreateDefaultAudioSource(Transform parent)
    {
        var source = new GameObject("DefaultAudioSource").AddComponent<AudioSource>();
        source.transform.SetParent(parent);
        source.playOnAwake = false;
        return source;
    }
}
