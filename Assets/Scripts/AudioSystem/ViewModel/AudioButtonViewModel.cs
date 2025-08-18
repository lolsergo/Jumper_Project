using UniRx;
using Zenject;

public class AudioButtonViewModel
{
    public readonly ReactiveCommand<string> OnSoundRequested = new();

    [Inject]
    private AudioManager _audioManager;

    public AudioButtonViewModel()
    {
        OnSoundRequested.Subscribe(soundId => _audioManager.Play(soundId));
    }
}
