using UniRx;
using UnityEngine;

public class GameAudioSettings
{
    public ReactiveDictionary<AudioLibrary.AudioCategory, float> Volumes { get; } = new();

    private readonly IUserProfileService _profiles;

    public GameAudioSettings(IUserProfileService profiles)
    {
        _profiles = profiles;

        if (_profiles.CurrentSave.Value != null)
            LoadFromSave(_profiles.CurrentSave.Value);

        // Когда меняется профиль — подгружаем громкости
        _profiles.CurrentSave
            .Where(s => s != null)
            .Subscribe(LoadFromSave);

        // Когда меняется громкость — сохраняем в профиль
        Volumes.ObserveReplace()
            .Subscribe(change =>
            {
                var save = _profiles.CurrentSave.Value;
                if (save == null) return;

                switch (change.Key)
                {
                    case AudioLibrary.AudioCategory.SFX:
                        save.volumeSFX = change.NewValue; break;
                    case AudioLibrary.AudioCategory.Music:
                        save.volumeMusic = change.NewValue; break;
                    case AudioLibrary.AudioCategory.UI:
                        save.volumeUI = change.NewValue; break;
                }
                _profiles.SaveCurrent();
            });
    }

    private void LoadFromSave(SaveData save)
    {
        Volumes[AudioLibrary.AudioCategory.SFX] = save.volumeSFX;
        Volumes[AudioLibrary.AudioCategory.Music] = save.volumeMusic;
        Volumes[AudioLibrary.AudioCategory.UI] = save.volumeUI;
    }

    public float GetVolume(AudioLibrary.AudioCategory category)
        => Volumes.TryGetValue(category, out var v) ? v : 1f;
}