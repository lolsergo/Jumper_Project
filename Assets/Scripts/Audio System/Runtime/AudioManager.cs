using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public class AudioManager : IInitializable, IDisposable
{
    private readonly AudioLibrary _library;
    private readonly AudioPoolRegistry _poolRegistry;
    private readonly AudioSettings _settings;

    private readonly Dictionary<AudioSource, SoundID> _activeSources = new();
    private readonly Dictionary<AudioSource, IDisposable> _perSourceVolumeSubs = new(); // ← новое
    private readonly CompositeDisposable _disposables = new();

    [Inject]
    public AudioManager(AudioLibrary library, AudioPoolRegistry poolRegistry, AudioSettings settings)
    {
        _library = library;
        _poolRegistry = poolRegistry;
        _settings = settings;
    }

    public AudioSource Play(SoundID soundId, bool loop = false, float pitch = 1f, float speed = 1f)
    {
        var sound = _library.GetSound(soundId);
        if (sound?.Clip == null)
        {
            return null;
        }

        if (!_poolRegistry.Pools.TryGetValue(sound.Category, out var pool))
        {
            return null;
        }

        var source = pool.Spawn();
        source.clip = sound.Clip;
        source.loop = loop;

        float finalPitch = Mathf.Max(0.01f, pitch * speed);
        source.pitch = finalPitch;

        source.volume = sound.BaseVolume * _settings.GetVolume(sound.Category);

        _activeSources[source] = soundId;

        var volSub = _settings.Volumes.ObserveReplace()
            .Where(x => x.Key == sound.Category)
            .Subscribe(x =>
            {
                if (source != null)
                    source.volume = sound.BaseVolume * x.NewValue;
            });
        _perSourceVolumeSubs[source] = volSub;

        source.Play();

        if (!loop)
        {
            float duration = sound.Clip.length / finalPitch;
            Observable.Timer(TimeSpan.FromSeconds(duration))
                .Subscribe(_ => Stop(source))
                .AddTo(_disposables);
        }

        return source;
    }

    public AudioSource PlayFromGroup(string groupId, bool loop = false, float pitch = 1f, float speed = 1f)
    {
        var s = _library.GetRandomFromGroup(groupId);
        if (s == null)
        {
            return null;
        }

        var src = Play(s.ID, loop, pitch, speed);
        if (src != null)
        {
            var group = _library.GetGroup(groupId);
            if (group != null)
                src.volume *= group.VolumeMultiplier;
        }
        return src;
    }

    public void ModifySound(AudioSource source, float newPitch, float newSpeed)
    {
        if (source == null) return;
        source.pitch = Mathf.Max(0.01f, newPitch * newSpeed);
    }

    public void Stop(AudioSource source)
    {
        if (source == null) return;

        if (_perSourceVolumeSubs.TryGetValue(source, out var sub))
        {
            sub.Dispose();
            _perSourceVolumeSubs.Remove(source);
        }

        if (_activeSources.TryGetValue(source, out var soundId))
        {
            var sound = _library.GetSound(soundId);
            if (sound != null && _poolRegistry.Pools.TryGetValue(sound.Category, out var pool))
            {
                source.Stop();
                pool.Despawn(source);
            }
            _activeSources.Remove(source);
        }
    }

    public void Initialize() => Debug.Log("[Audio] Manager initialized");
    public void Dispose()
    {
        foreach (var kv in _perSourceVolumeSubs) kv.Value?.Dispose();
        _perSourceVolumeSubs.Clear();
        _disposables.Dispose();
    }
}