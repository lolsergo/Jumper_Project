using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

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
        private bool _enabled;

        public void Enable()
        {
            if (_enabled) return;

            if (_inputAction?.action == null)
            {
                Debug.LogWarning($"[{nameof(InputController)}] InputAction for {Type} is not assigned");
                return;
            }

            _startedHandler = _ => OnPressed?.Invoke();
            _canceledHandler = _ => OnReleased?.Invoke();
            _performedHandler = _ =>
            {
                // For button actions (with interactions) Performed can be raised repeatedly.
                if (_inputAction.action.phase == InputActionPhase.Performed)
                    OnHolding?.Invoke();
            };

            var action = _inputAction.action;
            action.started += _startedHandler;
            action.canceled += _canceledHandler;
            action.performed += _performedHandler;
            action.Enable();
            _enabled = true;
        }

        public void Disable()
        {
            if (!_enabled) return;
            if (_inputAction?.action == null) return;

            var action = _inputAction.action;
            if (_startedHandler != null) action.started -= _startedHandler;
            if (_canceledHandler != null) action.canceled -= _canceledHandler;
            if (_performedHandler != null) action.performed -= _performedHandler;
            action.Disable();

            _startedHandler = null;
            _canceledHandler = null;
            _performedHandler = null;
            _enabled = false;
        }
    }

    public enum InputActionType
    {
        Jump,
        Pause
    }

    [Header("Configuration")]
    [SerializeField] private bool _autoEnable = true;
    [SerializeField] private List<ActionEvent> _actions = new();

    private readonly Dictionary<InputActionType, InputActionState> _states = new();
    private readonly Dictionary<InputActionType, (Action pressed, Action released, Action holding)> _internalHandlers
        = new();

    public ActionEvent GetAction(InputActionType type)
    {
        var action = _actions.FirstOrDefault(a => a.Type == type);
        if (action == null)
        {
            Debug.LogError($"Action {type} not found! Check {nameof(InputController)} setup");
            action = CreateFallbackAction(type);
        }
        EnsureState(type, action);
        return action;
    }

    public InputActionState GetState(InputActionType type)
    {
        if (!_states.TryGetValue(type, out var s))
        {
            EnsureState(type, GetAction(type));
            s = _states[type];
        }
        return s;
    }

    public bool WasPressedThisFrame(InputActionType type) => GetState(type).Pressed;
    public bool WasReleasedThisFrame(InputActionType type) => GetState(type).Released;
    public bool IsHolding(InputActionType type) => GetState(type).Holding;

    public void EnableAll()
    {
        foreach (var a in _actions)
            a.Enable();
    }

    public void DisableAll()
    {
        foreach (var a in _actions)
            a.Disable();
        foreach (var s in _states.Values)
        {
            s.Pressed = false;
            s.Released = false;
            s.Holding = false;
        }
    }

    private void Awake()
    {
        BuildStatesAndWire();
    }

    private void OnEnable()
    {
        if (_autoEnable)
            EnableAll();
    }

    private void OnDisable()
    {
        DisableAll();
    }

    private void LateUpdate()
    {
        foreach (var s in _states.Values)
            s.ResetFrameStates();
    }

    private void OnDestroy()
    {
        UnwireInternalHandlers();
    }

    private ActionEvent CreateFallbackAction(InputActionType type)
    {
        var fallback = new ActionEvent { Type = type };
        _actions.Add(fallback);
        return fallback;
    }

    private void BuildStatesAndWire()
    {
        UnwireInternalHandlers();
        _states.Clear();
        _internalHandlers.Clear();

        foreach (var action in _actions)
            EnsureState(action.Type, action);
    }

    private void EnsureState(InputActionType type, ActionEvent actionEvent)
    {
        if (_states.ContainsKey(type))
            return;

        var state = new InputActionState();
        _states.Add(type, state);

        Action pressed = () =>
        {
            state.Pressed = true;
            state.Holding = true;
        };
        Action released = () =>
        {
            state.Released = true;
            state.Holding = false;
        };
        Action holding = () =>
        {
            state.Holding = true;
        };

        actionEvent.OnPressed += pressed;
        actionEvent.OnReleased += released;
        actionEvent.OnHolding += holding;

        _internalHandlers[type] = (pressed, released, holding);
    }

    private void UnwireInternalHandlers()
    {
        foreach (var action in _actions)
        {
            if (_internalHandlers.TryGetValue(action.Type, out var tuple))
            {
                action.OnPressed -= tuple.pressed;
                action.OnReleased -= tuple.released;
                action.OnHolding -= tuple.holding;
            }
        }
        _internalHandlers.Clear();
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

        if (Application.isPlaying == false)
        {
            BuildStatesAndWire();
        }
    }
#endif
}