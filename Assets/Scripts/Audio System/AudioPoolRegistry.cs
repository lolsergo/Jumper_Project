using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class AudioPoolRegistry : IInitializable
{
    public Dictionary<AudioLibrary.AudioCategory, AudioPool> Pools { get; } = new();

    private readonly DiContainer _container;
    private readonly AudioCoreInstaller.Settings _settings;

    [Inject]
    public AudioPoolRegistry(DiContainer container, AudioCoreInstaller.Settings settings)
    {
        _container = container;
        _settings = settings;
    }

    public void Initialize()
    {
        var parent = new GameObject("AudioPools").transform;
        Object.DontDestroyOnLoad(parent.gameObject);

        foreach (AudioLibrary.AudioCategory category in System.Enum.GetValues(typeof(AudioLibrary.AudioCategory)))
        {
            Pools[category] = new AudioPool(_container, _settings.SourcePrefab, parent);
        }

    }
}