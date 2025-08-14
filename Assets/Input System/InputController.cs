using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections.Generic;

public class InputController : MonoBehaviour
{
    [Serializable]
    public class ActionEvent
    {
        public string name;
        public InputActionReference inputAction;
        public event Action OnPressed;
        public event Action OnReleased;
        public event Action OnHolding;

        // Методы для вызова событий (только внутри этого класса)
        public void InvokePressed() => OnPressed?.Invoke();
        public void InvokeReleased() => OnReleased?.Invoke();
        public void InvokeHolding() => OnHolding?.Invoke();

        public void Enable()
        {
            if (inputAction == null) return;

            inputAction.action.started += _ => InvokePressed();
            inputAction.action.canceled += _ => InvokeReleased();
            inputAction.action.Enable();
        }

        public void Disable()
        {
            if (inputAction == null) return;

            inputAction.action.started -= _ => InvokePressed();
            inputAction.action.canceled -= _ => InvokeReleased();
            inputAction.action.Disable();
        }
    }

    public List<ActionEvent> actions = new();

    private void Update()
    {
        if (actions.Count != 0)
        {
            foreach (var action in actions)
            {
                if (action.inputAction?.action.IsPressed() ?? false)
                {
                    action.InvokeHolding(); // Теперь правильно!
                }
            }
        }
    }

    private void OnEnable() => actions.ForEach(a => a.Enable());
    private void OnDisable() => actions.ForEach(a => a.Disable());
}