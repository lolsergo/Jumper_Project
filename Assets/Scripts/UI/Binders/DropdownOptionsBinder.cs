using MVVM;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public sealed class DropdownOptionsBinder : IBinder
{
    private readonly TMP_Dropdown _dropdown;
    private readonly List<string> _options;

    public DropdownOptionsBinder(TMP_Dropdown dropdown, List<string> options)
    {
        _dropdown = dropdown;
        _options = options;
    }

    public void Bind()
    {
        _dropdown.options.Clear();
        foreach (var option in _options)
        {
            _dropdown.options.Add(new TMP_Dropdown.OptionData(option));
        }
        _dropdown.RefreshShownValue();
    }

    public void Unbind()
    {

    }
}