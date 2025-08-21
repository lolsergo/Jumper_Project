using UnityEngine;

public class SettingsService : ISettingsService
{
    public float MusicVolume { get; set; } = 1f;
    public Resolution ScreenResolution { get; set; } = Screen.currentResolution;
    public bool FullScreen { get; set; } = true;

    public SettingsService()
    {
        Load();
    }

    public void Save()
    {
        PlayerPrefs.SetFloat("MusicVolume", MusicVolume);
        PlayerPrefs.SetInt("FullScreen", FullScreen ? 1 : 0);
        PlayerPrefs.SetInt("ResolutionIndex", GetResolutionIndex(ScreenResolution));
        PlayerPrefs.Save();
    }

    private void Load()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
            MusicVolume = PlayerPrefs.GetFloat("MusicVolume");

        if (PlayerPrefs.HasKey("FullScreen"))
            FullScreen = PlayerPrefs.GetInt("FullScreen") == 1;

        if (PlayerPrefs.HasKey("ResolutionIndex"))
        {
            var res = Screen.resolutions;
            int idx = PlayerPrefs.GetInt("ResolutionIndex");
            if (idx >= 0 && idx < res.Length)
                ScreenResolution = res[idx];
        }
    }

    private int GetResolutionIndex(Resolution res)
    {
        var arr = Screen.resolutions;
        for (int i = 0; i < arr.Length; i++)
            if (arr[i].width == res.width && arr[i].height == res.height) return i;
        return -1;
    }
}