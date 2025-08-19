using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections.Generic;
using System.Linq;

public class InputController : MonoBehaviour
{
    [Serializable]
    public class ActionEvent
    {
        [SerializeField] private InputActionReference _inputAction;
        public InputActionReference InputAction => _inputAction;

        public InputActionType Type;

        public event Action OnPressed;
        public event Action OnReleased;
        public event Action OnHolding;

        private Action<InputAction.CallbackContext> _startedHandler;
        private Action<InputAction.CallbackContext> _canceledHandler;
        private Action<InputAction.CallbackContext> _performedHandler;

        public void Enable()
        {
            if (_inputAction?.action == null)
            {
                Debug.LogWarning($"[{nameof(InputController)}] InputAction for {Type} is not assigned");
                return;
            }

            _startedHandler = _ => OnPressed?.Invoke();
            _canceledHandler = _ => OnReleased?.Invoke();
            _performedHandler = ctx =>
            {
                if (_inputAction.action.phase == InputActionPhase.Performed)
                    OnHolding?.Invoke();
            };

            var action = _inputAction.action;
            action.started += _startedHandler;
            action.canceled += _canceledHandler;
            action.performed += _performedHandler;
            action.Enable();
        }

        public void Disable()
        {
            if (_inputAction?.action == null) return;

            var action = _inputAction.action;
            if (_startedHandler != null) action.started -= _startedHandler;
            if (_canceledHandler != null) action.canceled -= _canceledHandler;
            if (_performedHandler != null) action.performed -= _performedHandler;
            action.Disable();
        }
    }

    public enum InputActionType
    {
        Jump,
        Pause
    }

    [SerializeField] private List<ActionEvent> _actions = new();

    public ActionEvent GetAction(InputActionType type)
    {
        var action = _actions.FirstOrDefault(a => a.Type == type);
        if (action == null)
        {
            Debug.LogError($"Action {type} not found! Check {nameof(InputController)} setup");
            return CreateFallbackAction(type);
        }
        return action;
    }

    private ActionEvent CreateFallbackAction(InputActionType type)
    {
        var fallback = new ActionEvent { Type = type };
        _actions.Add(fallback);
        return fallback;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        var duplicates = _actions
            .GroupBy(a => a.Type)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key);

        foreach (var duplicate in duplicates)
            Debug.LogError($"Duplicate action type found: {duplicate}");

        foreach (InputActionType type in Enum.GetValues(typeof(InputActionType)))
        {
            if (!_actions.Any(a => a.Type == type))
                Debug.LogWarning($"Action {type} is missing in inspector!");
        }
    }
#endif
}