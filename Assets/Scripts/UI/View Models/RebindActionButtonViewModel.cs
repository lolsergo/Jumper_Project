using System;
using UniRx;
using MVVM;
using Zenject;

public class RebindActionButtonViewModel : IDisposable
{
    public InputController.InputActionType ActionType { get; }
    public int BindingIndex { get; }

    [Data("StartRebind")]
    public readonly ReactiveCommand StartRebind = new();

    [Data("ResetBinding")]
    public readonly ReactiveCommand ResetBinding = new();

    [Data("DisplayName")]
    public readonly ReactiveProperty<string> DisplayName = new(string.Empty);

    [Data("IsRebinding")]
    public readonly ReactiveProperty<bool> IsRebinding = new(false);

    [Data("CommandName")]
    public readonly ReactiveProperty<string> CommandName = new(string.Empty);

    private readonly IInputRebindService _rebindService;
    private readonly CompositeDisposable _disposables = new();

    [Inject]
    public RebindActionButtonViewModel(
        IInputRebindService rebindService,
        InputController.InputActionType actionType,
        int bindingIndex = 0)
    {
        _rebindService = rebindService;
        ActionType = actionType;
        BindingIndex = bindingIndex;

        CommandName.Value = ActionType.ToString();

        Refresh();

        StartRebind
            .Subscribe(_ => BeginRebind())
            .AddTo(_disposables);

        ResetBinding
            .Subscribe(_ =>
            {
                _rebindService.ResetBinding(ActionType, BindingIndex);
                Refresh();
            })
            .AddTo(_disposables);

        _rebindService.RebindStarted += OnRebindStarted;
        _rebindService.RebindCompleted += OnRebindCompleted;
        _rebindService.RebindCanceled += OnRebindCanceled;
    }

    private void BeginRebind()
    {
        if (IsRebinding.Value) return;

        if (string.IsNullOrEmpty(DisplayName.Value))
            Refresh();

        IsRebinding.Value = true;
        _rebindService.StartRebind(
            ActionType,
            BindingIndex,
            exclude: null,
            onComplete: display =>
            {
                DisplayName.Value = display;
                IsRebinding.Value = false;
            },
            onCancel: () =>
            {
                IsRebinding.Value = false;
            });
    }

    private void OnRebindStarted(InputController.InputActionType type)
    {
        if (type == ActionType)
            IsRebinding.Value = true;
    }

    private void OnRebindCompleted(InputController.InputActionType type, string display)
    {
        if (type != ActionType) return;
        DisplayName.Value = display;
        IsRebinding.Value = false;
    }

    private void OnRebindCanceled(InputController.InputActionType type)
    {
        if (type != ActionType) return;
        IsRebinding.Value = false;
        Refresh();
    }

    public void Refresh()
    {
        DisplayName.Value = _rebindService.GetBindingDisplayName(ActionType, BindingIndex);
    }

    public void Dispose()
    {
        _rebindService.RebindStarted -= OnRebindStarted;
        _rebindService.RebindCompleted -= OnRebindCompleted;
        _rebindService.RebindCanceled -= OnRebindCanceled;
        _disposables.Dispose();
    }

    public class Factory : PlaceholderFactory<InputController.InputActionType, int, RebindActionButtonViewModel> { }
}