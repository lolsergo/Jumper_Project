using UnityEngine.SceneManagement;
using UnityEngine;
using UniRx;
using Zenject;
using System;

public class GameManager : IInitializable, IDisposable
{
    private readonly UIManager _uiManager;
    private readonly IHealthService _healthService;
    private readonly CompositeDisposable _disposables = new();
    private bool _isPaused;

    [Inject]
    public GameManager(UIManager uiManager, IHealthService healthService)
    {
        _uiManager = uiManager;
        _healthService = healthService;
    }

    public void Initialize()
    {
        _healthService.OnDeath
            .Subscribe(_ =>
            {
                Debug.Log("GameManager: death received, Lose()");
                Lose();
            });
    }

    public void Dispose() => _disposables.Dispose();

    public void PauseGame()
    {
        _isPaused = true;
        Time.timeScale = 0f;
        _uiManager.ShowPauseScreen();
    }

    public void ResumeGame()
    {
        _isPaused = false;
        Time.timeScale = 1f;
        _uiManager.HidePauseScreen();
    }

    public void Lose()
    {
        Time.timeScale = 0f;
        _uiManager.ShowLoseScreen();
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        _uiManager.ShowGameOverScreen();
    }

    public void ReturnToMainMenu()
    {
        GameEvents.OnGameCleanup.OnNext(Unit.Default);
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    public void BuyHealth(int amount)
    {
        _healthService.AddHealth(amount);
        Time.timeScale = 1f;
        _uiManager.HideLoseScreen();
    }


    public void HandlePauseInput()
    {
        if (_isPaused) ResumeGame();
        else PauseGame();
    }
}