using UniRx;
using Zenject;
using System;
using MVVM;

public sealed class PauseButtonViewModel : IInitializable, IDisposable
{
    [Data("PauseClick")]
    public readonly Action PauseAction;

    private readonly GameplayService _gameManager;
    private readonly CompositeDisposable _disposables = new();

    [Inject]
    public PauseButtonViewModel(GameplayService gameManager)
    {
        _gameManager = gameManager;
        PauseAction = () => {
            _gameManager.HandlePauseInput();
        };
    }

    public void Initialize() { }
    public void Dispose() => _disposables.Dispose();
}

