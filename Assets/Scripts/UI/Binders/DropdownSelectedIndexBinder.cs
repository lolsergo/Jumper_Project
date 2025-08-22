using MVVM;
using TMPro;
using System;

public sealed class DropdownSelectedIndexBinder : IBinder
{
    private readonly TMP_Dropdown _dropdown;
    private readonly Func<int> _getIndex;
    private readonly Action<int> _setIndex;

    private bool _ignoreCallback;

    public DropdownSelectedIndexBinder(TMP_Dropdown dropdown, Func<int> getIndex, Action<int> setIndex)
    {
        _dropdown = dropdown;
        _getIndex = getIndex;
        _setIndex = setIndex;
    }

    public void Bind()
    {
        _dropdown.value = _getIndex();

        _dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    public void Unbind()
    {
        _dropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
    }

    private void OnDropdownValueChanged(int newIndex)
    {
        if (_ignoreCallback)
            return;

        _setIndex(newIndex);
    }
}