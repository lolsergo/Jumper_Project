using System;

public interface IInputRebindService
{
    event Action<InputController.InputActionType> RebindStarted;
    event Action<InputController.InputActionType, string> RebindCompleted;
    event Action<InputController.InputActionType> RebindCanceled;

    void Initialize();
    void StartRebind(InputController.InputActionType actionType, int bindingIndex = 0, string[] exclude = null,
        Action<string> onComplete = null, Action onCancel = null);
    void ResetBinding(InputController.InputActionType actionType, int bindingIndex = 0);
    void ResetAll();
    string GetBindingDisplayName(InputController.InputActionType actionType, int bindingIndex = 0);
}