using UniRx;
using Zenject;
using UnityEngine;
using System;
using MVVM;

public sealed class ResumeButtonViewModel : IInitializable, IDisposable
{
    [Data("ResumeClick")]
    public readonly Action ResumeAction;

    private readonly GameManager _gameManager;
    private readonly CompositeDisposable _disposables = new();

    [Inject]
    public ResumeButtonViewModel(GameManager gameManager)
    {
        _gameManager = gameManager;
        ResumeAction = () => {
            _gameManager.HandlePauseInput();
        };
    }

    public void Initialize() { }
    public void Dispose() => _disposables.Dispose();
}

