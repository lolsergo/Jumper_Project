using UnityEngine;
using Zenject;

public class AudioSourcePool : MemoryPool<AudioSource>
{
    protected override void OnCreated(AudioSource item)
    {
        item.playOnAwake = false;
        item.spatialBlend = 0; // 2D звук
    }

    protected override void OnSpawned(AudioSource item)
    {
        item.gameObject.SetActive(true);
    }

    protected override void OnDespawned(AudioSource item)
    {
        item.Stop();
        item.clip = null;
        item.gameObject.SetActive(false);
    }
}