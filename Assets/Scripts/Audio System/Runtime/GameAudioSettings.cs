using UniRx;
using UnityEngine;
using System;
using System.Collections.Generic;

public class GameAudioSettings
{
    public ReactiveDictionary<AudioLibrary.AudioCategory, float> Volumes { get; } = new();

    private readonly IUserProfileService _profileService;
    private readonly Dictionary<AudioLibrary.AudioCategory, Action<SaveData, float>> _saveSetters;

    public GameAudioSettings(IUserProfileService profileService)
    {
        _profileService = profileService ?? throw new ArgumentNullException(nameof(profileService));
        _saveSetters = CreateSaveSetters();

        InitializeVolumes();
        LoadInitialSave();
        SubscribeToSaveChanges();
        SubscribeToVolumeChanges();
    }

    public float GetVolume(AudioLibrary.AudioCategory category) =>
        Volumes.TryGetValue(category, out var v) ? v : 1f;

    public void SetVolume(AudioLibrary.AudioCategory category, float value)
    {
        if (Volumes.TryGetValue(category, out var current) && Mathf.Approximately(current, value))
            return;

        Volumes[category] = Mathf.Clamp01(value);
    }
    public void ApplyTo(AudioSource source, AudioLibrary.Sound sound)
    {
        if (source == null || sound == null) return;
        source.volume = sound.BaseVolume * GetVolume(sound.Category);
    }

    private void InitializeVolumes()
    {
        foreach (AudioLibrary.AudioCategory cat in Enum.GetValues(typeof(AudioLibrary.AudioCategory)))
            Volumes[cat] = 1f;
    }

    private void LoadInitialSave()
    {
        var save = _profileService.CurrentSave.Value;
        if (save != null)
            LoadFromSave(save);
    }

    private void SubscribeToSaveChanges()
    {
        _profileService.CurrentSave
            .Where(s => s != null)
            .Subscribe(LoadFromSave);
    }

    private void SubscribeToVolumeChanges()
    {
        Volumes.ObserveReplace()
            .Subscribe(change =>
            {
                var save = _profileService.CurrentSave.Value;
                if (save == null) return;

                if (_saveSetters.TryGetValue(change.Key, out var setter))
                    setter(save, change.NewValue);

                _profileService.SaveCurrent();
            });
    }

    private static Dictionary<AudioLibrary.AudioCategory, Action<SaveData, float>> CreateSaveSetters() =>
        new()
        {
            { AudioLibrary.AudioCategory.SFX,   (s, v) => s.sfxVolume   = v },
            { AudioLibrary.AudioCategory.Music, (s, v) => s.musicVolume = v },
            { AudioLibrary.AudioCategory.UI,    (s, v) => s.uiVolume    = v },
        };

    private void LoadFromSave(SaveData save)
    {
        if (save == null) return;
        SetVolume(AudioLibrary.AudioCategory.SFX, save.sfxVolume);
        SetVolume(AudioLibrary.AudioCategory.Music, save.musicVolume);
        SetVolume(AudioLibrary.AudioCategory.UI, save.uiVolume);
    }
}