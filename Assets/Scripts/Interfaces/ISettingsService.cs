using UnityEngine;

public interface ISettingsService
{
    Resolution ScreenResolution { get; set; }
    bool FullScreen { get; set; }

    void Save();
}
