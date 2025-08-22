using MVVM;
using System;

public sealed class DropdownActionBinder : IBinder
{
    private readonly Action<Action<int>> _setViewAction;
    private readonly Func<Action<int>> _getViewModelAction;

    public DropdownActionBinder(Action<Action<int>> setViewAction, Func<Action<int>> getViewModelAction)
    {
        _setViewAction = setViewAction;
        _getViewModelAction = getViewModelAction;
    }

    public void Bind()
    {
        _setViewAction?.Invoke(_getViewModelAction());
    }

    public void Unbind()
    {
        _setViewAction?.Invoke(null);
    }
}