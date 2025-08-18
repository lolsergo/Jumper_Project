using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Audio Model")]
public class AudioModel : ScriptableObject
{
    [System.Serializable]
    public class Sound
    {
        public string Id;  // "UI_Click", "Health_Buy"
        public AudioClip Clip;
        [Range(0f, 1f)] public float Volume = 1f;
    }

    public Sound[] Sounds;
}