using UnityEngine;
using Zenject;
using System.Collections.Generic;

public class AudioPoolRegistry : IInitializable
{
    private readonly DiContainer _container;
    private readonly AudioSource _audioSourcePrefab;
    private Transform _poolsRoot;

    public Dictionary<AudioLibrary.AudioCategory, AudioSourcePool> Pools { get; }
        = new Dictionary<AudioLibrary.AudioCategory, AudioSourcePool>();

    public AudioPoolRegistry(DiContainer container, AudioSource audioSourcePrefab)
    {
        _container = container;
        _audioSourcePrefab = audioSourcePrefab;
    }

    public void Initialize()
    {
        _poolsRoot = new GameObject("[AudioPools]").transform;
        Object.DontDestroyOnLoad(_poolsRoot.gameObject);

        foreach (AudioLibrary.AudioCategory category in System.Enum.GetValues(typeof(AudioLibrary.AudioCategory)))
        {
            var poolParent = new GameObject($"Pool_{category}").transform;
            poolParent.SetParent(_poolsRoot);

            var pool = _container.Instantiate<AudioSourcePool>();
            _container.BindMemoryPool<AudioSource, AudioSourcePool>()
                .WithInitialSize(2)
                .FromComponentInNewPrefab(_audioSourcePrefab)
                .UnderTransform(poolParent);

            Pools.Add(category, pool);
        }
    }
}