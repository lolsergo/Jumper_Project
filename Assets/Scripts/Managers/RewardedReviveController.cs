using System;
using Zenject;
using UnityEngine;

public class RewardedReviveController : IInitializable, IDisposable
{
    private readonly IAdService _ads;
    private readonly GameManager _gameManager;

    private ReviveState _state = ReviveState.Idle;
    private bool _rewardEarnedFlag;

    public event Action AvailabilityChanged;

    // Can show revive option when player just died (AwaitingDecision) and ad ready.
    public bool CanRevive => _state == ReviveState.AwaitingDecision && _ads.IsRewardedReady;

    [Inject]
    public RewardedReviveController(IAdService ads, GameManager gameManager)
    {
        _ads = ads;
        _gameManager = gameManager;
    }

    public void Initialize()
    {
        _ads.RewardedLoaded += OnAdLoaded;
        _ads.RewardEarned += OnRewardEarned;
        _ads.RewardedClosed += OnAdClosed;
        _gameManager.PlayerDied += OnPlayerDied;
    }

    // Optional external reset if a new session reuses same instance.
    public void Reset()
    {
        bool availabilityBefore = CanRevive;
        _state = ReviveState.Idle;
        _rewardEarnedFlag = false;
        if (availabilityBefore != CanRevive)
            AvailabilityChanged?.Invoke();
    }

    private void OnPlayerDied()
    {
        if (_state != ReviveState.Idle) return;

        _state = ReviveState.AwaitingDecision;
        NotifyAvailabilityIfChanged();
    }

    public bool TryShow()
    {
        if (!CanRevive) return false;
        return _ads.ShowRewarded();
    }

    private void OnAdLoaded()
    {
        // Only matters if we are waiting and previously not ready.
        if (_state == ReviveState.AwaitingDecision)
            AvailabilityChanged?.Invoke();
    }

    private void OnRewardEarned()
    {
        // Ad network signaled reward; actual revive will be applied after closure.
        _rewardEarnedFlag = true;
    }

    private void OnAdClosed()
    {
        if (_rewardEarnedFlag && _state == ReviveState.AwaitingDecision)
        {
            _gameManager.RevivePlayer();
            _state = ReviveState.Consumed;
            _rewardEarnedFlag = false;
            AvailabilityChanged?.Invoke(); // Now definitely unavailable.
            return;
        }

        // No reward; still awaiting decision? (User skipped or failed ad)
        if (_state == ReviveState.AwaitingDecision)
        {
            // Availability may have changed if ad can’t be replayed immediately.
            NotifyAvailabilityIfChanged();
        }
    }

    private void NotifyAvailabilityIfChanged()
    {
        AvailabilityChanged?.Invoke();
    }

    public void Dispose()
    {
        _ads.RewardedLoaded -= OnAdLoaded;
        _ads.RewardEarned -= OnRewardEarned;
        _ads.RewardedClosed -= OnAdClosed;
        _gameManager.PlayerDied -= OnPlayerDied;
    }

    private enum ReviveState
    {
        Idle,            // Player alive or pre-death.
        AwaitingDecision, // Player died; can potentially show revive.
        Consumed          // Revive already used this session.
    }
}