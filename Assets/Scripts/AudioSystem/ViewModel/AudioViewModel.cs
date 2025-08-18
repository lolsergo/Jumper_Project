using UniRx;
using Zenject;
using MVVM;

public sealed class AudioEventSystem : IInitializable
{
    [Data("PlaySound")] // Ключ для биндинга!
    public readonly ReactiveCommand<string> OnSoundRequested = new();

    private readonly AudioManager _manager;
    private CompositeDisposable _disposables = new();

    [Inject]
    public AudioEventSystem(AudioManager manager)
    {
        _manager = manager;
    }

    public void Initialize()
    {
        OnSoundRequested.Subscribe(soundID => _manager.Play(soundID))
            .AddTo(_disposables);
    }

    public void Dispose() => _disposables.Clear();
}