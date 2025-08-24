using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public class AudioProvider : IInitializable, IDisposable
{
    private readonly AudioLibrary _library;
    private readonly AudioPoolRegistry _poolRegistry;
    private readonly GameAudioSettings _settings;

    private readonly Dictionary<AudioSource, SoundID> _activeSources = new();
    private readonly Dictionary<AudioSource, IDisposable> _perSourceVolumeSubs = new();
    private readonly CompositeDisposable _disposables = new();

    [Inject]
    public AudioProvider(AudioLibrary library, AudioPoolRegistry poolRegistry, GameAudioSettings settings)
    {
        _library = library;
        _poolRegistry = poolRegistry;
        _settings = settings;
    }

    public AudioSource Play(SoundID soundId, bool loop = false, float pitch = 1f, float speed = 1f)
    {
        var sound = _library.GetSound(soundId);
        return PlaySound(sound, loop, pitch, speed);
    }

    public AudioSource PlayFromGroup(SoundGroupID groupId, bool loop = false, float pitch = 1f, float speed = 1f)
    {
        var s = _library.GetRandomFromGroup(groupId);
        return PlaySound(s, loop, pitch, speed, groupId);
    }

    public AudioSource PlayFromGroupByIndex(SoundGroupID groupId, int index, bool loop = false, float pitch = 1f, float speed = 1f)
    {
        var s = _library.GetFromGroupByIndex(groupId, index);
        return PlaySound(s, loop, pitch, speed, groupId);
    }

    public AudioSource PlaySound(AudioLibrary.Sound sound, bool loop = false, float pitch = 1f, float speed = 1f, SoundGroupID? groupId = null)
    {
        if (sound?.Clip == null) return null;
        if (!_poolRegistry.Pools.TryGetValue(sound.Category, out var pool))
            return null;

        var source = pool.Spawn();
        source.clip = sound.Clip;
        source.loop = loop;

        float finalPitch = Mathf.Max(0.01f, pitch * Mathf.Max(speed, 0.01f));
        source.pitch = finalPitch;

        _settings.ApplyTo(source, sound);

        if (groupId.HasValue)
        {
            var group = _library.GetGroup(groupId.Value);
            if (group != null)
                source.volume *= group.VolumeMultiplier;
        }

        _activeSources[source] = sound.ID;

        var volSub = _settings.Volumes.ObserveReplace()
            .Where(x => x.Key == sound.Category)
            .Subscribe(_ => _settings.ApplyTo(source, sound));
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

    public void ModifySound(AudioSource source, float newPitch, float newSpeed)
    {
        if (source == null) return;
        source.pitch = Mathf.Max(0.01f, newPitch * Mathf.Max(newSpeed, 0.01f));
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