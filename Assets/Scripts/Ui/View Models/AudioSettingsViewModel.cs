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

    private readonly GameAudioSettings _audioSettings;
    private readonly CompositeDisposable _disposables = new();

    [Inject]
    public AudioSettingsViewModel(GameAudioSettings audioSettings)
    {
        _audioSettings = audioSettings;
    }

    public void Initialize()
    {
        SFXVolume.Value   = _audioSettings.GetVolume(AudioLibrary.AudioCategory.SFX);
        MusicVolume.Value = _audioSettings.GetVolume(AudioLibrary.AudioCategory.Music);
        UIVolume.Value    = _audioSettings.GetVolume(AudioLibrary.AudioCategory.UI);

        SFXVolume.Skip(1)
            .Subscribe(v => _audioSettings.SetVolume(AudioLibrary.AudioCategory.SFX, v))
            .AddTo(_disposables);
        MusicVolume.Skip(1)
            .Subscribe(v => _audioSettings.SetVolume(AudioLibrary.AudioCategory.Music, v))
            .AddTo(_disposables);
        UIVolume.Skip(1)
            .Subscribe(v => _audioSettings.SetVolume(AudioLibrary.AudioCategory.UI, v))
            .AddTo(_disposables);
    }

    public void Dispose() => _disposables.Dispose();
}