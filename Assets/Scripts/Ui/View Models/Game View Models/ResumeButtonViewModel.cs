using UniRx;
using Zenject;
using System;
using MVVM;

public sealed class ResumeButtonViewModel : IInitializable, IDisposable
{
    [Data("ResumeClick")]
    public readonly Action ResumeAction;

    private readonly GameplayService _gameManager;
    private readonly CompositeDisposable _disposables = new();

    [Inject]
    public ResumeButtonViewModel(GameplayService gameManager)
    {
        _gameManager = gameManager;
        ResumeAction = () => {
            _gameManager.HandlePauseInput();
        };
    }

    public void Initialize() { }
    public void Dispose() => _disposables.Dispose();
}

