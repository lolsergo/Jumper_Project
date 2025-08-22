using UnityEngine;

public interface ISettingsService
{
    float SFXVolume { get; set; }
    float MusicVolume { get; set; }
    float UIVolume { get; set; }
    Resolution ScreenResolution { get; set; }
    bool FullScreen { get; set; }

    void Save();
}
