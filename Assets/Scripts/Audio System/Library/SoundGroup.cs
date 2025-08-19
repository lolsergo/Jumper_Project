using UnityEngine;

[CreateAssetMenu(menuName = "Audio/SoundGroup", fileName = "NewSoundGroup")]
public class SoundGroup : ScriptableObject
{
    public string GroupId = "Footstep_Default";
    public AudioLibrary.AudioCategory Category = AudioLibrary.AudioCategory.SFX;
    public SoundID[] Variants;            // Список SoundID, каждый есть в AudioLibrary
    [Range(0f, 1f)] public float VolumeMultiplier = 1f;
}