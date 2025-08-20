using MVVM;
using System;
using UniRx;
using Zenject;

public sealed class DisplayDistanceViewModel : IInitializable, IDisposable, IObserver<string>
{
    [Data("Distance")]
    public readonly ReactiveProperty<string> Distance = new ();

    private readonly GameSpeedManager _speedManager;
    private IDisposable _subscription;

    public DisplayDistanceViewModel(GameSpeedManager speedManager)
    {
        _speedManager = speedManager;
    }

    public void Initialize()
    {
        _subscription = _speedManager.DistanceReached
            .Select(d => d.ToString("F2")) // или .ToString() для int
            .Subscribe(this);
    }

    public void Dispose()
    {
        _subscription?.Dispose();
    }

    // Реализация IObserver<string>
    public void OnNext(string value) => Distance.Value = value;
    public void OnCompleted() { }
    public void OnError(Exception error) { }
}