using UnityEngine;

public class AudioSourceWrapper : MonoBehaviour
{
    public AudioSource Source { get; private set; }

    void Awake()
    {
        Source = gameObject.AddComponent<AudioSource>();
        Source.playOnAwake = false;
    }
}
