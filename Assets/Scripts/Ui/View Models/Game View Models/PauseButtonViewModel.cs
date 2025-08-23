using UniRx;
using Zenject;
using UnityEngine;
using System;
using MVVM;

public sealed class PauseButtonViewModel : IInitializable, IDisposable
{
    [Data("PauseClick")]
    public readonly Action PauseAction;

    private readonly GameManager _gameManager;
    private readonly CompositeDisposable _disposables = new();

    [Inject]
    public PauseButtonViewModel(GameManager gameManager)
    {
        _gameManager = gameManager;
        PauseAction = () => {
            _gameManager.HandlePauseInput();
        };
    }

    public void Initialize() { }
    public void Dispose() => _disposables.Dispose();
}

