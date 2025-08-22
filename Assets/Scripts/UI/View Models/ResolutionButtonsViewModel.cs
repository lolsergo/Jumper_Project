using UniRx;
using Zenject;
using UnityEngine;
using System;
using MVVM;
using System.Linq;

public sealed class ResolutionButtonsViewModel : IInitializable, IDisposable
{
    [Data("SetResolution1")]
    public readonly Action SetResolution1Action;
    [Data("SetResolution2")]
    public readonly Action SetResolution2Action;
    [Data("SetResolution3")]
    public readonly Action SetResolution3Action;

    public readonly Resolution[] AvailableResolutions = new Resolution[3];

    [Data("Resolution1Text")]
    public readonly string Resolution1Text;
    [Data("Resolution2Text")]
    public readonly string Resolution2Text;
    [Data("Resolution3Text")]
    public readonly string Resolution3Text;

    private readonly ISettingsService _settingsService;
    private readonly CompositeDisposable _disposables = new();

    [Inject]
    public ResolutionButtonsViewModel(ISettingsService settingsService)
    {
        _settingsService = settingsService;

        var resolutions = Screen.resolutions
            .Select(r => new { r.width, r.height })
            .Distinct()
            .OrderByDescending(r => r.width * r.height)
            .Take(3)
            .Select(r => new Resolution { width = r.width, height = r.height })
            .ToArray();

        for (int i = 0; i < AvailableResolutions.Length; i++)
            AvailableResolutions[i] = i < resolutions.Length ? resolutions[i] : new Resolution { width = 800, height = 600 };

        Resolution1Text = $"{AvailableResolutions[0].width}x{AvailableResolutions[0].height}";
        Resolution2Text = $"{AvailableResolutions[1].width}x{AvailableResolutions[1].height}";
        Resolution3Text = $"{AvailableResolutions[2].width}x{AvailableResolutions[2].height}";

        SetResolution1Action = () => ApplyResolution(AvailableResolutions[0]);
        SetResolution2Action = () => ApplyResolution(AvailableResolutions[1]);
        SetResolution3Action = () => ApplyResolution(AvailableResolutions[2]);
    }

    private void ApplyResolution(Resolution res)
    {
        _settingsService.ScreenResolution = res;
        Screen.SetResolution(res.width, res.height, _settingsService.FullScreen);
        _settingsService.Save();
        Debug.Log($"[VM] Resolution set: {res.width}x{res.height}");
    }

    public void Initialize() => Debug.Log("[VM] ResolutionButtonsViewModel Initialized");
    public void Dispose() => _disposables.Dispose();
}