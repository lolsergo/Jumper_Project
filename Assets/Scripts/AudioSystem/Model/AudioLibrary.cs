using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Library")]
public class AudioLibrary : ScriptableObject
{
    [System.Serializable]
    public class Sound
    {
        public string ID;
        public AudioClip Clip;
        [Range(0f, 1f)] public float BaseVolume = 1f;
        public AudioCategory Category;
    }

    public enum AudioCategory { SFX, Music, UI }
    public Sound[] Sounds;

    public Sound GetSound(string id) => System.Array.Find(Sounds, s => s.ID == id);
}
