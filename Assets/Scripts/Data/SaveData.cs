[System.Serializable]
public class SaveData
{
    public string profileName;
    public float maxDistanceReached;
    public float totalPlayTime;
    public int tries;

    public float sfxVolume = 1f;
    public float musicVolume = 1f;
    public float uiVolume = 1f;

    public bool fullscreen = true;
    public int resolutionIndex = 0;
}