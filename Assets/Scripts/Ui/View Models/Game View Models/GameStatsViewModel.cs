using System;
using UniRx;
using Zenject;
using MVVM;
using UnityEngine;

public class GameStatsViewModel : IInitializable, IDisposable
{
    [Data("MaxDistance")]
    public readonly ReactiveProperty<string> MaxDistance = new();

    [Data("Tries")]
    public readonly ReactiveProperty<string> Tries = new();

    [Data("PlayTime")]
    public readonly ReactiveProperty<string> PlayTime = new();

    private readonly IUserProfileService _saveProfile;
    private IDisposable _subscription;

    public GameStatsViewModel(IUserProfileService saveProfile)
    {
        _saveProfile = saveProfile;
    }

    public void Initialize()
    {
        _subscription = _saveProfile.CurrentSave
            .Where(s => s != null)
            .Subscribe(s =>
            {
                MaxDistance.Value = s.maxDistanceReached.ToString("F2");
                Tries.Value = s.tries.ToString();
                PlayTime.Value = FormatTime(s.totalPlayTime);
            });

        // Инициализация начальными значениями
        var current = _saveProfile.CurrentSave.Value;
        if (current != null)
        {
            MaxDistance.Value = current.maxDistanceReached.ToString("F2");
            Tries.Value = current.tries.ToString();
            PlayTime.Value = FormatTime(current.totalPlayTime);
        }
    }

    private string FormatTime(float seconds)
    {
        var ts = TimeSpan.FromSeconds(seconds);
        return $"{(int)ts.TotalMinutes:D2}:{ts.Seconds:D2}";
    }

    public void Dispose() => _subscription?.Dispose();
}
