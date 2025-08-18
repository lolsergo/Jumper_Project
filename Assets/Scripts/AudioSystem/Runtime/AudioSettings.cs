using UniRx;

public class AudioSettings
{
    public ReactiveDictionary<AudioLibrary.AudioCategory, float> Volumes { get; }
        = new ReactiveDictionary<AudioLibrary.AudioCategory, float>
        {
            [AudioLibrary.AudioCategory.SFX] = 1f,
            [AudioLibrary.AudioCategory.Music] = 0.8f,
            [AudioLibrary.AudioCategory.UI] = 0.9f
        };
}
