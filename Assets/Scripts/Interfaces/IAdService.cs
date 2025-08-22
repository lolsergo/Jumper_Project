using System;

public interface IAdService
{
    bool IsRewardedReady { get; }
    event Action RewardedLoaded;
    event Action RewardedFailedToLoad;
    event Action RewardedClosed;
    event Action RewardEarned;

    void Initialize();
    void LoadRewarded();
    bool ShowRewarded();
}
