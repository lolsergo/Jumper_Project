using System;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdMobService : IAdService, IDisposable
{
    private const string TestRewardedId = "ca-app-pub-3940256099942544/5224354917";

    public bool IsRewardedReady => _rewardedAdLoaded;

    public event Action RewardedLoaded;
    public event Action RewardedFailedToLoad;
    public event Action RewardedClosed;
    public event Action RewardEarned;

    private readonly string _rewardedUnitId;

    private RewardedAd _rewarded;
    private bool _rewardedAdLoaded;
    private bool _initialized;
    private bool _isLoading;

    // Backoff
    private int _retryAttempt;
    private float _nextRetryTime;

    // Глобальный mute
    private bool _audioMuted;
    private float _savedListenerVolume = 1f;

    public AdMobService( string rewardedUnitId = null)
    {
        _rewardedUnitId = string.IsNullOrWhiteSpace(rewardedUnitId) ? TestRewardedId : rewardedUnitId;
    }

    public void Initialize()
    {
        if (_initialized) return;

        MobileAds.Initialize(_ =>
        {
            _initialized = true;
            _retryAttempt = 0;
            LoadRewarded();
        });
    }

    public void LoadRewarded()
    {
        if (!_initialized) return;
        if (_isLoading) return;
        if (Time.realtimeSinceStartup < _nextRetryTime) return;

        _isLoading = true;
        _rewardedAdLoaded = false;

        _rewarded?.Destroy();
        _rewarded = null;

        var request = new AdRequest();

        RewardedAd.Load(_rewardedUnitId, request, (ad, error) =>
        {
            _isLoading = false;

            if (error != null || ad == null)
            {
                string errMsg = error != null ? error.GetMessage() : "Ad object null";
                Debug.LogWarning($"[AdMob] Rewarded load failed (attempt {_retryAttempt + 1}): {errMsg}");
                _retryAttempt++;
                float delay = Mathf.Min(Mathf.Pow(2f, _retryAttempt), 60f);
                _nextRetryTime = Time.realtimeSinceStartup + delay;
                RewardedFailedToLoad?.Invoke();
                return;
            }

            _retryAttempt = 0;
            _nextRetryTime = 0f;
            _rewarded = ad;
            _rewardedAdLoaded = true;
            AttachCallbacks(ad);
            RewardedLoaded?.Invoke();
        });
    }

    public bool ShowRewarded()
    {
        if (!_rewardedAdLoaded || _rewarded == null)
            return false;

        _rewardedAdLoaded = false;

        _rewarded.Show(reward =>
        {
            Debug.Log($"[AdMob] Reward earned: {reward.Amount} {reward.Type}");
            RewardEarned?.Invoke();
        });

        return true;
    }

    private void AttachCallbacks(RewardedAd ad)
    {
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("[AdMob] Rewarded opened");
            MuteAudio();
        };

        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("[AdMob] Rewarded closed");
            UnmuteAudio();
            RewardedClosed?.Invoke();
            LoadRewarded();
        };

        ad.OnAdFullScreenContentFailed += err =>
        {
            Debug.LogWarning("[AdMob] Rewarded show failed: " + err.GetMessage());
            UnmuteAudio();
            RewardedClosed?.Invoke();
            LoadRewarded();
        };

        ad.OnAdImpressionRecorded += () => { /* optional analytics */ };

        ad.OnAdPaid += value =>
        {
            // optional monetization tracking
        };
    }

    private void MuteAudio()
    {
        if (_audioMuted) return;
        _savedListenerVolume = AudioListener.volume;
        AudioListener.volume = 0f;
        _audioMuted = true;
    }

    private void UnmuteAudio()
    {
        if (!_audioMuted) return;
        AudioListener.volume = _savedListenerVolume;
        _audioMuted = false;
    }

    public void Dispose()
    {
        UnmuteAudio();
        _rewarded?.Destroy();
        _rewarded = null;
    }
}