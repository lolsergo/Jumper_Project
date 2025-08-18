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
        // 1. ������� ������������ ������ ���� �� ������
        var audioRoot = parent != null ? parent : new GameObject("AudioSystem").transform;

        // 2. ������� AudioSource ���� �� ������ ������
        var audioSource = sourcePrefab != null
            ? _container.InstantiatePrefabForComponent<AudioSource>(sourcePrefab, audioRoot)
            : CreateDefaultAudioSource(audioRoot);

        // 3. ������������ �����������
        _container.BindInstance(library).AsSingle();
        _container.BindInstance(audioSource).AsSingle();

        // 4. ������� � ���������� AudioManager
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
