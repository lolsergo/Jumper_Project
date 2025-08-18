using System;
using UniRx;
using UnityEngine;
using Zenject;

public class AudioManager : IInitializable, IDisposable
{
    private readonly AudioLibrary _library;
    private readonly AudioPoolRegistry _poolRegistry;
    private readonly CompositeDisposable _disposables = new();

    [Inject]
    public AudioManager(AudioLibrary library, AudioPoolRegistry poolRegistry)
    {
        _library = library;
        _poolRegistry = poolRegistry;
    }

    public void Play(string soundId)
    {
        var sound = _library.GetSound(soundId);
        if (sound?.Clip == null)
        {
            Debug.LogWarning($"Sound not found or clip is null: {soundId}");
            return;
        }

        if (!_poolRegistry.Pools.TryGetValue(sound.Category, out var pool))
        {
            Debug.LogError($"No pool for category: {sound.Category}");
            return;
        }

        var source = pool.Spawn();
        source.PlayOneShot(sound.Clip, sound.BaseVolume);

        Observable.Timer(TimeSpan.FromSeconds(sound.Clip.length))
            .Subscribe(_ => pool.Despawn(source))
            .AddTo(_disposables);
    }

    public void Initialize() => Debug.Log("AudioManager initialized");
    public void Dispose() => _disposables.Dispose();
}