using UniRx;
using UnityEngine;

public class AudioSettings
{
    public ReactiveDictionary<AudioLibrary.AudioCategory, float> Volumes { get; }
        = new ReactiveDictionary<AudioLibrary.AudioCategory, float>
        {
            [AudioLibrary.AudioCategory.SFX] = 1f,
            [AudioLibrary.AudioCategory.Music] = 0.8f,
            [AudioLibrary.AudioCategory.UI] = 0.9f
        };

    public void SetVolume(AudioLibrary.AudioCategory category, float volume)
    {
        if (Volumes.ContainsKey(category))
            Volumes[category] = Mathf.Clamp01(volume);
    }

    public float GetVolume(AudioLibrary.AudioCategory category)
    {
        return Volumes.TryGetValue(category, out var volume) ? volume : 1f;
    }
}
