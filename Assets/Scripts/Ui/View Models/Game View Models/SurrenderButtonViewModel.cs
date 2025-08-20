using UniRx;
using UnityEngine;
using System;
using Zenject;
using MVVM;

public class SurrenderButtonViewModel
{
    // Изменяем property на поле с атрибутом
    [Data("SurrenderButton")]
    public readonly Action PauseAction;

    private readonly GameManager _gameManager;
    private readonly CompositeDisposable _disposables = new();

    [Inject]
    public SurrenderButtonViewModel(GameManager gameManager)
    {
        _gameManager = gameManager;
        PauseAction = () => {
            _gameManager.GameOver();
        };
    }

    public void Initialize() => Debug.Log("[VM] Initialized");
    public void Dispose() => _disposables.Dispose();
}
