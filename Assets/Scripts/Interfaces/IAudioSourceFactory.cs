using UnityEngine;

public interface IAudioSourceFactory
{
    AudioSource Create(AudioLibrary.AudioCategory category);
}
