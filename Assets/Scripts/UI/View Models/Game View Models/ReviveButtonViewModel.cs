using System;
using Zenject;
using UniRx;
using MVVM;

public sealed class ReviveButtonViewModel : IInitializable, IDisposable
{
    private readonly RewardedReviveController _controller;
    private readonly CompositeDisposable _disposables = new();

    // ������� ������
    [Data("ReviveButton")]
    public readonly Action Revive;

    // ��������� (Reactive) ��� ��������
    [Data("ReviveButtonVisible")]
    public readonly ReactiveProperty<bool> IsVisible = new(false);

    [Inject]
    public ReviveButtonViewModel(RewardedReviveController controller)
    {
        _controller = controller;
        Revive = () => _controller.TryShow();
    }

    public void Initialize()
    {
        _controller.AvailabilityChanged += OnAvailabilityChanged;
        Evaluate();
    }

    private void OnAvailabilityChanged() => Evaluate();

    private void Evaluate()
    {
        bool can = _controller.CanRevive;
        if (IsVisible.Value != can)
            IsVisible.Value = can;
    }

    public void Dispose()
    {
        _controller.AvailabilityChanged -= OnAvailabilityChanged;
        _disposables.Dispose();
    }
}