using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class InputRebindService : IInputRebindService, IInitializable, IDisposable
{
    private readonly InputController _inputController;
    private readonly IUserProfileService _profileService;

    private readonly Dictionary<InputController.InputActionType, InputAction> _cachedActions =
        new Dictionary<InputController.InputActionType, InputAction>();

    private InputActionRebindingExtensions.RebindingOperation _currentOperation;

    public event Action<InputController.InputActionType> RebindStarted;
    public event Action<InputController.InputActionType, string> RebindCompleted;
    public event Action<InputController.InputActionType> RebindCanceled;

    private bool _disableWholeMap;
    private bool _initialized;

    [Inject]
    public InputRebindService(InputController inputController, IUserProfileService profileService)
    {
        _inputController = inputController;
        _profileService = profileService;
    }

    public void Initialize()
    {
        if (_initialized) return;
        CacheActions();
        ApplySavedOverrides();
        _initialized = true;
    }

    public void Dispose()
    {
        _currentOperation?.Cancel();
        _currentOperation?.Dispose();
    }

    private void CacheActions()
    {
        foreach (InputController.InputActionType type in Enum.GetValues(typeof(InputController.InputActionType)))
        {
            if (_cachedActions.ContainsKey(type)) continue;
            var actionEvent = _inputController.GetAction(type);
            var action = actionEvent?.InputAction?.action;
            if (action != null)
                _cachedActions.Add(type, action);
        }
    }

    private SaveData CurrentSave => _profileService.CurrentSave.Value;

    public void StartRebind(
    InputController.InputActionType actionType,
    int bindingIndex = 0,
    string[] exclude = null,
    Action<string> onComplete = null,
    Action onCancel = null)
    {
        // Ensure initialized (covers cases when Initialize not called explicitly yet).
        Initialize();

        if (_currentOperation != null)
        {
            Debug.LogWarning("[InputRebindService] A rebind is already in progress.");
            onCancel?.Invoke();
            return;
        }

        bool saveEnabled = CurrentSave != null;

        if (_cachedActions.Count == 0)
            CacheActions();

        var action = GetAction(actionType);
        if (action == null)
        {
            Debug.LogError($"[InputRebindService] Action {actionType} not found (missing InputActionReference).");
            onCancel?.Invoke();
            return;
        }

        if (bindingIndex < 0 || bindingIndex >= action.bindings.Count)
        {
            Debug.LogError($"[InputRebindService] Invalid bindingIndex {bindingIndex} for {actionType}.");
            onCancel?.Invoke();
            return;
        }

        bool actionWasEnabled = action.enabled;
        bool mapWasEnabled = false;
        var map = action.actionMap;

        try
        {
            if (_disableWholeMap && map != null)
            {
                mapWasEnabled = map.enabled;
                if (mapWasEnabled) map.Disable();
            }
            else if (actionWasEnabled)
            {
                action.Disable();
            }

            RebindStarted?.Invoke(actionType);

            var op = action.PerformInteractiveRebinding(bindingIndex)
                .WithMatchingEventsBeingSuppressed(true)
                .WithCancelingThrough("<Keyboard>/escape");

            if (exclude != null)
            {
                foreach (var e in exclude)
                    op.WithControlsExcluding(e);
            }

            op.OnCancel(operation =>
            {
                operation.Dispose();
                _currentOperation = null;
                RestoreEnableState(action, map, actionWasEnabled, mapWasEnabled);
                RebindCanceled?.Invoke(actionType);
                onCancel?.Invoke();
            });

            op.OnComplete(operation =>
            {
                operation.Dispose();
                _currentOperation = null;

                var overridePath = action.bindings[bindingIndex].overridePath;
                if (string.IsNullOrEmpty(overridePath))
                    overridePath = action.bindings[bindingIndex].effectivePath;

                if (saveEnabled)
                {
                    CurrentSave.SetOrReplaceBinding(actionType.ToString(), bindingIndex, overridePath);
                    _profileService.SaveCurrent();
                }

                var display = action.GetBindingDisplayString(bindingIndex);
                RebindCompleted?.Invoke(actionType, display);
                onComplete?.Invoke(display);

                RestoreEnableState(action, map, actionWasEnabled, mapWasEnabled);
            });

            _currentOperation = op.Start();
        }
        catch (Exception ex)
        {
            Debug.LogError($"[InputRebindService] Failed to start rebind for {actionType}: {ex.Message}");
            _currentOperation?.Dispose();
            _currentOperation = null;
            RestoreEnableState(action, map, actionWasEnabled, mapWasEnabled);
            onCancel?.Invoke();
        }
    }

    private void RestoreEnableState(InputAction action, InputActionMap map, bool actionWasEnabled, bool mapWasEnabled)
    {
        if (_disableWholeMap)
        {
            if (mapWasEnabled && map != null && !map.enabled) map.Enable();
        }
        else
        {
            if (actionWasEnabled && !action.enabled) action.Enable();
        }
    }

    public void ResetBinding(InputController.InputActionType actionType, int bindingIndex = 0)
    {
        var action = GetAction(actionType);
        if (action == null) return;
        if (bindingIndex < 0 || bindingIndex >= action.bindings.Count) return;

        action.RemoveBindingOverride(bindingIndex);

        if (CurrentSave != null)
        {
            CurrentSave.SetOrReplaceBinding(actionType.ToString(), bindingIndex, null);
            _profileService.SaveCurrent();
        }
    }

    public void ResetAll()
    {
        foreach (var kv in _cachedActions)
        {
            var action = kv.Value;
            for (int i = 0; i < action.bindings.Count; i++)
                action.RemoveBindingOverride(i);
        }

        if (CurrentSave != null)
        {
            CurrentSave.inputBindings.Clear();
            _profileService.SaveCurrent();
        }
    }

    public string GetBindingDisplayName(InputController.InputActionType actionType, int bindingIndex = 0)
    {
        var action = GetAction(actionType);
        if (action == null) return string.Empty;
        if (bindingIndex < 0 || bindingIndex >= action.bindings.Count) return string.Empty;
        return action.GetBindingDisplayString(bindingIndex);
    }

    private InputAction GetAction(InputController.InputActionType type)
    {
        if (_cachedActions.TryGetValue(type, out var action))
            return action;

        var actionEvent = _inputController.GetAction(type);
        action = actionEvent?.InputAction?.action;
        if (action != null)
            _cachedActions[type] = action;
        return action;
    }

    private void ApplySavedOverrides()
    {
        if (CurrentSave == null) return;
        if (CurrentSave.inputBindings == null) return;

        foreach (var entry in CurrentSave.inputBindings.ToList())
        {
            if (!Enum.TryParse(entry.actionType, out InputController.InputActionType type))
                continue;

            var action = GetAction(type);
            if (action == null) continue;
            if (entry.bindingIndex < 0 || entry.bindingIndex >= action.bindings.Count) continue;

            try
            {
                action.ApplyBindingOverride(entry.bindingIndex, entry.overridePath);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[InputRebindService] Failed applying override for {entry.actionType} index {entry.bindingIndex}: {ex.Message}");
            }
        }
    }
}
