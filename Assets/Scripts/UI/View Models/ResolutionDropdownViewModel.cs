using Zenject;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public sealed class ResolutionDropdownViewModel : IInitializable, IDisposable
{
    public readonly Resolution[] Resolutions;
    public int SelectedResolutionIndex;
    public Action<int> OnResolutionChanged;

    private readonly ISettingsService _settingsService;

    [Inject]
    public ResolutionDropdownViewModel(ISettingsService settingsService)
    {
        _settingsService = settingsService;

        // Собираем уникальные (width,height), берем вариант с наибольшей частотой
        Resolutions = Screen.resolutions
            .GroupBy(r => (r.width, r.height))
            .Select(g => g.OrderByDescending(r => r.refreshRateRatio.value).First())
            .OrderByDescending(r => r.width * r.height)
            .ToArray();

        var current = _settingsService.ScreenResolution;
        SelectedResolutionIndex = Array.FindIndex(
            Resolutions,
            r => r.width == current.width && r.height == current.height
        );
        if (SelectedResolutionIndex < 0)
            SelectedResolutionIndex = 0;

        // Назначаем обработчик СРАЗУ (до биндинга)
        OnResolutionChanged = (index) =>
        {
            if (index < 0 || index >= Resolutions.Length) return;
            var res = Resolutions[index];
            _settingsService.ScreenResolution = res;
            Screen.SetResolution(res.width, res.height, _settingsService.FullScreen);
            _settingsService.Save();
            SelectedResolutionIndex = index;
            Debug.Log($"[ResolutionVM] Applied {res.width}x{res.height}");
        };
    }

    // Можно оставить пустым
    public void Initialize() { }

    public void Dispose() { }
}