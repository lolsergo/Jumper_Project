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
        [SerializeField] 
        private InputActionReference _inputAction;
        public InputActionReference InputAction => _inputAction;

        public InputActionType Type;

        public event Action OnPressed;
        public event Action OnReleased;
        public event Action OnHolding;

        public void Enable()
        {
            if (_inputAction == null) return;

            _inputAction.action.started += _ => OnPressed?.Invoke();
            _inputAction.action.canceled += _ => OnReleased?.Invoke();
            _inputAction.action.Enable();
        }

        public void Disable()
        {
            if (_inputAction == null) return;

            _inputAction.action.started -= _ => OnPressed?.Invoke();
            _inputAction.action.canceled -= _ => OnReleased?.Invoke();
            _inputAction.action.Disable();
        }
    }

    public enum InputActionType
    {
        Jump,
        Pause
    }

    [SerializeField] 
    private List<ActionEvent> _actions = new();

    private void OnEnable() => _actions.ForEach(a => a.Enable());
    private void OnDisable() => _actions.ForEach(a => a.Disable());

    // Основной безопасный метод доступа
    public ActionEvent GetAction(InputActionType type)
    {
        var action = _actions.FirstOrDefault(a => a.Type == type);

        if (action == null)
        {
            Debug.LogError($"Action {type} not found! Check InputController setup.");
            return CreateFallbackAction(type);
        }

        return action;
    }

    private ActionEvent CreateFallbackAction(InputActionType type)
    {
        Debug.LogWarning($"Creating fallback action for {type}");
        var fallback = new ActionEvent { Type = type };
        _actions.Add(fallback);
        return fallback;
    }

#if UNITY_EDITOR
    // Автоматическая валидация в редакторе
    private void OnValidate()
    {
        // Проверяем дубликаты
        var duplicates = _actions
            .GroupBy(a => a.Type)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key);

        foreach (var duplicate in duplicates)
        {
            Debug.LogError($"Duplicate action type found: {duplicate}");
        }

        // Проверяем все ли enum значения покрыты
        foreach (InputActionType type in Enum.GetValues(typeof(InputActionType)))
        {
            if (!_actions.Any(a => a.Type == type))
            {
                Debug.LogWarning($"Action {type} is missing in inspector!");
            }
        }
    }
#endif
}