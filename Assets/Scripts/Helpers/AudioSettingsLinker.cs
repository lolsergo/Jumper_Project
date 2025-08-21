using System;
using UniRx;
using UnityEditor.Overlays;
using UnityEngine;
using Zenject;

public sealed class AudioSettingsLinker : IInitializable, IDisposable
{
    private readonly IUserProfileService _profileService;
    private readonly AudioSettings _audioSettings;
    private readonly CompositeDisposable _disposables = new();

    [Inject]
    public AudioSettingsLinker(IUserProfileService profileService, AudioSettings audioSettings)
    {
        _profileService = profileService;
        _audioSettings = audioSettings;
    }

    public void Initialize()
    {
        // 1. При смене текущего сохранения — применяем громкости
        _profileService.CurrentSave
            .Where(save => save != null)
            .Subscribe(ApplyAudioFromSave)
            .AddTo(_disposables);

        // 2. При изменении громкости — пишем в сохранение
        _audioSettings.Volumes.ObserveReplace()
            .Subscribe(x =>
            {
                var save = _profileService.CurrentSave.Value;
                if (save == null) return;

                switch (x.Key)
                {
                    case AudioLibrary.AudioCategory.SFX:
                        save.volumeSFX = x.NewValue; break;
                    case AudioLibrary.AudioCategory.Music:
                        save.volumeMusic = x.NewValue; break;
                    case AudioLibrary.AudioCategory.UI:
                        save.volumeUI = x.NewValue; break;
                }

                _profileService.SaveCurrent();
            })
            .AddTo(_disposables);
    }

    private void ApplyAudioFromSave(SaveData save)
    {
        _audioSettings.SetVolume(AudioLibrary.AudioCategory.SFX, save.volumeSFX);
        _audioSettings.SetVolume(AudioLibrary.AudioCategory.Music, save.volumeMusic);
        _audioSettings.SetVolume(AudioLibrary.AudioCategory.UI, save.volumeUI);
    }

    public void Dispose() => _disposables.Dispose();
}