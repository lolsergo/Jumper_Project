using UniRx;
using UnityEngine;
using System;
using System.Collections.Generic;

public class GameAudioSettings
{
    public ReactiveDictionary<AudioLibrary.AudioCategory, float> Volumes { get; } = new();

    private readonly IUserProfileService _profiles;
    private readonly Dictionary<AudioLibrary.AudioCategory, Action<SaveData, float>> _saveSetters;

    public GameAudioSettings(IUserProfileService profiles)
    {
        _profiles = profiles;

        // Маппинг для записи в SaveData
        _saveSetters = new Dictionary<AudioLibrary.AudioCategory, Action<SaveData, float>>
        {
            { AudioLibrary.AudioCategory.SFX,   (s, v) => s.volumeSFX   = v },
            { AudioLibrary.AudioCategory.Music, (s, v) => s.volumeMusic = v },
            { AudioLibrary.AudioCategory.UI,    (s, v) => s.volumeUI    = v },
        };

        // Инициализация всех ключей по умолчанию
        foreach (AudioLibrary.AudioCategory cat in Enum.GetValues(typeof(AudioLibrary.AudioCategory)))
            Volumes[cat] = 1f;

        // Начальная загрузка
        if (_profiles.CurrentSave.Value != null)
            LoadFromSave(_profiles.CurrentSave.Value);

        // Реакция на смену активного сейва
        _profiles.CurrentSave
            .Where(s => s != null)
            .Subscribe(LoadFromSave);

        // Автосохранение при изменениях
        Volumes.ObserveReplace()
            .Subscribe(change =>
            {
                var save = _profiles.CurrentSave.Value;
                if (save == null) return;

                if (_saveSetters.TryGetValue(change.Key, out var setter))
                    setter(save, change.NewValue);

                _profiles.SaveCurrent();
            });
    }

    public float GetVolume(AudioLibrary.AudioCategory category) =>
        Volumes.TryGetValue(category, out var v) ? v : 1f;

    public void SetVolume(AudioLibrary.AudioCategory category, float value)
    {
        if (Volumes.ContainsKey(category) && Mathf.Approximately(Volumes[category], value))
            return; // чтобы не триггерить событие без изменений

        Volumes[category] = Mathf.Clamp01(value);
    }

    public void ApplyTo(AudioSource source, AudioLibrary.Sound sound)
    {
        if (source == null || sound == null) return;
        source.volume = sound.BaseVolume * GetVolume(sound.Category);
    }

    private void LoadFromSave(SaveData save)
    {
        SetVolume(AudioLibrary.AudioCategory.SFX, save.volumeSFX);
        SetVolume(AudioLibrary.AudioCategory.Music, save.volumeMusic);
        SetVolume(AudioLibrary.AudioCategory.UI, save.volumeUI);
    }
}