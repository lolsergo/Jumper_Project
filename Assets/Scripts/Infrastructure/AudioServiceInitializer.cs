using Zenject;
using UnityEngine;

public class AudioServiceInitializer : IInitializable
{
    private readonly AudioManager _audioManager;

    [Inject]
    public AudioServiceInitializer(AudioManager audioManager)
    {
        _audioManager = audioManager;
    }

    public void Initialize()
    {
        Debug.Log("[Audio] AudioServiceInitializer confirmed AudioManager is ready.");
    }
}
