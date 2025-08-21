using UnityEngine;

public interface ISettingsService
{
    float MusicVolume { get; set; }
    Resolution ScreenResolution { get; set; }
    bool FullScreen { get; set; }

    void Save();
}
