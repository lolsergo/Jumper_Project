using UnityEngine;
using System;
using UniRx;

public class SettingsService : ISettingsService, System.IDisposable
{
    // Только экранные настройки
    public Resolution ScreenResolution { get; set; } = Screen.currentResolution;
    public bool FullScreen { get; set; } = true;

    private readonly IUserProfileService _profileService;
    private IDisposable _profileSubscription;

    public SettingsService(IUserProfileService profileService)
    {
        _profileService = profileService ?? throw new ArgumentNullException(nameof(profileService));
        SubscribeToProfile();
        LoadFromProfile(_profileService.CurrentSave.Value);
    }

    private void SubscribeToProfile()
    {
        _profileSubscription = _profileService.CurrentSave
            .Where(s => s != null)
            .Subscribe(LoadFromProfile);
    }

    private void LoadFromProfile(SaveData save)
    {
        if (save == null) return;

        FullScreen = save.fullscreen;

        var resolutions = Screen.resolutions;
        if (save.resolutionIndex >= 0 && save.resolutionIndex < resolutions.Length)
            ScreenResolution = resolutions[save.resolutionIndex];
        else
            ScreenResolution = Screen.currentResolution;
    }

    public void Save()
    {
        var save = _profileService.CurrentSave.Value;
        if (save == null) return;

        save.fullscreen = FullScreen;
        save.resolutionIndex = GetResolutionIndex(ScreenResolution);

        _profileService.SaveCurrent();
    }

    private int GetResolutionIndex(Resolution res)
    {
        var arr = Screen.resolutions;
        for (int i = 0; i < arr.Length; i++)
            if (arr[i].width == res.width && arr[i].height == res.height)
                return i;
        return -1;
    }

    public void Dispose()
    {
        _profileSubscription?.Dispose();
    }
}