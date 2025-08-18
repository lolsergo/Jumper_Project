using UnityEngine;

[CreateAssetMenu(menuName = "Audio/AudioConfig")]
public class AudioConfig : ScriptableObject
{
    public AudioLibrary Library;
    public AudioSource AudioSourcePrefab;
    public Transform AudioSourcesParent;
}
