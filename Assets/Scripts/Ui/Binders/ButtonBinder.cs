using MVVM;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public sealed class ButtonBinder : IBinder
{
    private readonly Button _button;
    private readonly UnityAction _modelAction;
    private bool _isBound; // Флаг для отслеживания состояния

    public ButtonBinder(Button button, Action model)
    {
        _button = button;
        _modelAction = new UnityAction(model);
    }

    void IBinder.Bind()
    {
        if (_isBound) return; // Если уже подписаны — выходим

        _button.onClick.AddListener(_modelAction);
        _isBound = true;
    }

    public void Unbind()
    {
        if (!_isBound) return;

        _button.onClick.RemoveListener(_modelAction);
        _isBound = false;
    }
}