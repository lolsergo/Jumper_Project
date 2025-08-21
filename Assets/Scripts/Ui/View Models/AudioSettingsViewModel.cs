using MVVM;
using UniRx;
using Zenject;

public sealed class AudioSettingsViewModel : IInitializable, System.IDisposable
{
    [Data("SFX")]
    public readonly ReactiveProperty<float> SFXVolume = new();

    [Data("Music")]
    public readonly ReactiveProperty<float> MusicVolume = new();

    [Data("UI")]
    public readonly ReactiveProperty<float> UIVolume = new();

    private readonly GameAudioSettings _settings;
    private readonly CompositeDisposable _disposables = new();

    public AudioSettingsViewModel(GameAudioSettings settings)
    {
        _settings = settings;
    }

    public void Initialize()
    {
        SFXVolume.Value = _settings.GetVolume(AudioLibrary.AudioCategory.SFX);
        MusicVolume.Value = _settings.GetVolume(AudioLibrary.AudioCategory.Music);
        UIVolume.Value = _settings.GetVolume(AudioLibrary.AudioCategory.UI);

        SFXVolume.Skip(1).Subscribe(v => _settings.SetVolume(AudioLibrary.AudioCategory.SFX, v)).AddTo(_disposables);
        MusicVolume.Skip(1).Subscribe(v => _settings.SetVolume(AudioLibrary.AudioCategory.Music, v)).AddTo(_disposables);
        UIVolume.Skip(1).Subscribe(v => _settings.SetVolume(AudioLibrary.AudioCategory.UI, v)).AddTo(_disposables);

        _settings.Volumes.ObserveReplace()
            .Subscribe(change =>
            {
                switch (change.Key)
                {
                    case AudioLibrary.AudioCategory.SFX:
                        SFXVolume.Value = change.NewValue; break;
                    case AudioLibrary.AudioCategory.Music:
                        MusicVolume.Value = change.NewValue; break;
                    case AudioLibrary.AudioCategory.UI:
                        UIVolume.Value = change.NewValue; break;
                }
            })
            .AddTo(_disposables);
    }

    public void Dispose() => _disposables.Dispose();
}