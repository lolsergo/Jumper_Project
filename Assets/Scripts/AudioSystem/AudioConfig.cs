using UnityEngine;

[CreateAssetMenu(menuName = "Audio/AudioConfig")]
public class AudioConfig : ScriptableObject
{
    public float defaultVolume = 0.7f;
    public int initialPoolSize = 5;
}