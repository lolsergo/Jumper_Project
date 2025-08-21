[System.Serializable]
public class SaveData
{
    public string profileName;
    public float maxDistanceReached;
    public float totalPlayTime;
    public int tries;

    public float volumeSFX = 1f;
    public float volumeMusic = 1f;
    public float volumeUI = 1f;

    public bool fullscreen = true;
    public int resolutionIndex = 0;
}