using System;
using UniRx;
using Zenject;
using MVVM;
using UnityEngine;

public class MaxDistanceViewModel : IInitializable, IDisposable
{
    [Data("MaxDistance")]
    public readonly ReactiveProperty<string> Distance = new();

    private readonly IUserProfileService _saveProfile;
    private IDisposable _subscription;

    public MaxDistanceViewModel(IUserProfileService saveProfile)
    {
        _saveProfile = saveProfile;
    }

    public void Initialize()
    {
        _subscription = _saveProfile.CurrentSave
            .Where(s => s != null)
            .Subscribe(s => Distance.Value = s.maxDistanceReached.ToString("F2"));

        var current = _saveProfile.CurrentSave.Value;
        if (current != null)
            Distance.Value = current.maxDistanceReached.ToString("F2");
    }

    public void Dispose() => _subscription?.Dispose();
}
