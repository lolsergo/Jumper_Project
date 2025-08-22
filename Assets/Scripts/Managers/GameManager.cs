// ВСТАВКА изменений помечена комментариями // === REVIVE ===
using UnityEngine.SceneManagement;
using UnityEngine;
using UniRx;
using Zenject;
using System;

public class GameManager : IInitializable, IDisposable
{
    // === Dependencies ===
    private readonly UIManager _uiManager;
    private readonly IHealthService _healthService;
    private readonly GameSpeedManager _speedManager;
    private readonly IUserProfileService _profileService;
    private readonly CompositeDisposable _disposables = new();

    // === State ===
    private bool _isPaused;
    private bool _tryCountedThisRound;
    private float _sessionPlayTime;

    // === REVIVE ===
    public event Action PlayerDied;

    [Inject]
    public GameManager(
        UIManager uiManager,
        IHealthService healthService,
        GameSpeedManager speedManager,
        IUserProfileService profileService)
    {
        _uiManager = uiManager;
        _healthService = healthService;
        _speedManager = speedManager;
        _profileService = profileService;
    }

    public void Initialize()
    {
        ResetSessionStats();

        Observable.EveryUpdate()
            .Where(_ => Time.timeScale > 0f && !_isPaused)
            .Subscribe(_ => _sessionPlayTime += Time.deltaTime)
            .AddTo(_disposables);

        _healthService.OnDeath
            .Subscribe(_ =>
            {
                // Событие смерти до Lose (для revive контроллера)
                PlayerDied?.Invoke();
                Debug.Log("GameManager: death received, Lose()");
                Lose();
            })
            .AddTo(_disposables);
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
        UpdateMaxDistance();
        Time.timeScale = 0f;
        _uiManager.ShowLoseScreen();
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        UpdateMaxDistance();
        TryCommitSessionStats();
        _uiManager.ShowGameOverScreen();
    }

    public void ReturnToMainMenu()
    {
        TryCommitSessionStats();
        CleanupAndLoadMenu();
    }

    public void BuyHealth(int amount)
    {
        _healthService.AddHealth(amount);
        ResumeGame();
        _uiManager.HideLoseScreen();
    }

    // === REVIVE ===
    public void RevivePlayer()
    {
        // Простейший revive: вернуть 1 единицу HP (или AddHealth(1))
        _healthService.AddHealth(1);
        Time.timeScale = 1f;
        _uiManager.HideLoseScreen();
        Debug.Log("GameManager: Player revived via rewarded ad");
    }

    public void HandlePauseInput()
    {
        if (_isPaused) ResumeGame();
        else PauseGame();
    }

    // === Private helpers ===
    private void ResetSessionStats()
    {
        _tryCountedThisRound = false;
        _sessionPlayTime = 0f;
    }

    private void TryCommitSessionStats()
    {
        if (_tryCountedThisRound) return;

        var save = _profileService.CurrentSave.Value;
        if (save != null)
        {
            _profileService.IncrementTries();
            _profileService.AddPlayTime(_sessionPlayTime);

            (_profileService.CurrentSave as ReactiveProperty<SaveData>)
                ?.SetValueAndForceNotify(save);
        }

        _tryCountedThisRound = true;
    }

    private void UpdateMaxDistance()
    {
        var save = _profileService.CurrentSave.Value;
        if (save != null && _speedManager.CurrentDistance > save.maxDistanceReached)
        {
            save.maxDistanceReached = _speedManager.CurrentDistance;
            _profileService.SaveCurrent();

            (_profileService.CurrentSave as ReactiveProperty<SaveData>)
                ?.SetValueAndForceNotify(save);
        }
    }

    private void CleanupAndLoadMenu()
    {
        GameEvents.OnGameCleanup.OnNext(Unit.Default);
        Time.timeScale = 1f;
        SceneLoader.Load(SceneType.Menu);
    }
}