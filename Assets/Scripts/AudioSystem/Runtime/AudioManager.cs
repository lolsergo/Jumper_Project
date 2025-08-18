using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public class AudioManager : IInitializable, IDisposable
{
    private readonly AudioLibrary _library;
    private readonly AudioPoolRegistry _poolRegistry;
    private readonly Dictionary<AudioSource, string> _activeSources = new();
    private readonly CompositeDisposable _disposables = new();

    [Inject]
    public AudioManager(AudioLibrary library, AudioPoolRegistry poolRegistry)
    {
        _library = library;
        _poolRegistry = poolRegistry;
    }

    public AudioSource Play(string soundId, bool loop = false, float pitch = 1f, float speed = 1f)
    {
        var sound = _library.GetSound(soundId);
        if (sound?.Clip == null)
        {
            Debug.LogWarning($"Sound not found: {soundId}");
            return null;
        }

        if (!_poolRegistry.Pools.TryGetValue(sound.Category, out var pool))
        {
            Debug.LogError($"No pool for category: {sound.Category}");
            return null;
        }

        var source = pool.Spawn();
        source.clip = sound.Clip;
        source.volume = sound.BaseVolume;
        source.loop = loop;
        source.pitch = pitch * speed;

        _activeSources[source] = soundId;
        source.Play();

        if (!loop)
        {
            float duration = sound.Clip.length / speed;
            Observable.Timer(TimeSpan.FromSeconds(duration))
                .Subscribe(_ => Stop(source))
                .AddTo(_disposables);
        }

        return source;
    }

    public void Stop(AudioSource source)
    {
        if (source == null || !_activeSources.TryGetValue(source, out var soundId)) return;

        var sound = _library.GetSound(soundId);
        if (sound != null && _poolRegistry.Pools.TryGetValue(sound.Category, out var pool))
        {
            source.Stop();
            pool.Despawn(source);
            _activeSources.Remove(source);
        }
    }

    public void ModifySound(AudioSource source, float newPitch, float newSpeed)
    {
        if (source == null) return;
        source.pitch = newPitch * newSpeed;
    }

    public void Initialize() => Debug.Log("[Audio] Manager initialized");
    public void Dispose() => _disposables.Dispose();
}