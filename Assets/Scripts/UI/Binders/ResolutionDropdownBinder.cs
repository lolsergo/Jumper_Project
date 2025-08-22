using MVVM;
using TMPro;
using UnityEngine;

public sealed class ResolutionDropdownBinder : IBinder
{
    private readonly ResolutionDropdownView _view;
    private readonly ResolutionDropdownViewModel _vm;
    private TMP_Dropdown _dropdown;
    private bool _bound;

    public ResolutionDropdownBinder(ResolutionDropdownView view, ResolutionDropdownViewModel vm)
    {
        _view = view;
        _vm = vm;
    }

    public void Bind()
    {
        if (_bound) return;
        _dropdown = _view.Dropdown;
        if (_dropdown == null)
        {
            Debug.LogWarning("[ResolutionDropdownBinder] Dropdown is null");
            return;
        }

        Debug.Log("[ResolutionDropdownBinder] Bind start");

        RebuildOptions();

        int idx = Mathf.Clamp(_vm.SelectedResolutionIndex, 0, _dropdown.options.Count - 1);
        _dropdown.SetValueWithoutNotify(idx);

        _dropdown.onValueChanged.AddListener(OnChanged);
        _bound = true;
        Debug.Log($"[ResolutionDropdownBinder] Bound. Index={idx}, Options={_dropdown.options.Count}");
    }

    public void Unbind()
    {
        if (!_bound) return;
        if (_dropdown != null)
            _dropdown.onValueChanged.RemoveListener(OnChanged);
        _bound = false;
    }

    private void OnChanged(int index)
    {
        Debug.Log($"[ResolutionDropdownBinder] User selected index={index}");
        _vm.SelectedResolutionIndex = index;
        if (_vm.OnResolutionChanged == null)
            Debug.LogWarning("[ResolutionDropdownBinder] OnResolutionChanged is null (VM Initialize not run?)");
        _vm.OnResolutionChanged?.Invoke(index);
    }

    private void RebuildOptions()
    {
        _dropdown.options.Clear();
        foreach (var res in _vm.Resolutions)
        {
            _dropdown.options.Add(new TMP_Dropdown.OptionData(Format(res)));
        }
        _dropdown.RefreshShownValue();
    }

    private static string Format(Resolution r)
    {
        return $"{r.width}x{r.height} @ {r.refreshRateRatio.value}Hz";
    }
}
