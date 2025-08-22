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

    private readonly ISettingsService _settings;
    private readonly CompositeDisposable _disposables = new();

    [Inject]
    public AudioSettingsViewModel(ISettingsService settings)
    {
        _settings = settings;
    }

    public void Initialize()
    {
        SFXVolume.Value = _settings.SFXVolume;
        MusicVolume.Value = _settings.MusicVolume;
        UIVolume.Value = _settings.UIVolume;

        SFXVolume.Skip(1).Subscribe(v => { _settings.SFXVolume = v; _settings.Save(); }).AddTo(_disposables);
        MusicVolume.Skip(1).Subscribe(v => { _settings.MusicVolume = v; _settings.Save(); }).AddTo(_disposables);
        UIVolume.Skip(1).Subscribe(v => { _settings.UIVolume = v; _settings.Save(); }).AddTo(_disposables);
    }

    public void Dispose() => _disposables.Dispose();
}