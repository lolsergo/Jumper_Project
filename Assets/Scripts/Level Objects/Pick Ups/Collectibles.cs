using UnityEngine;
using Zenject;

public class Collectibles : LevelObject
{
    public event System.Action OnCollected;

    [SerializeField]
    private SoundGroupID collectSoundGroupId = SoundGroupID.Pickup_Default;
    [SerializeField]
    private int soundIndex = -1;

    protected AudioManager _audio;
    protected AudioLibrary _library;

    [Inject]
    private void Construct(AudioManager audio, AudioLibrary library)
    {
        _audio = audio;
        _library = library;
    }

    public override void Activate(Vector3 position)
    {
        if (_collider != null) _collider.enabled = true;
        if (_renderer != null) _renderer.enabled = true;
        base.Activate(position);
    }

    public override void Deactivate()
    {
        _collider.enabled = false;
        _renderer.enabled = false;
        base.Deactivate();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!gameObject.activeSelf || !other.CompareTag("Player"))
            return;

        Collect();
        Deactivate();
    }

    protected virtual void Collect()
    {
        PlayCollectSound();
        ApplyEffect();
        OnCollected?.Invoke();
    }

    protected void PlayCollectSound()
    {
        if (collectSoundGroupId == SoundGroupID.None)
            return;

        if (soundIndex < 0)
        {
            // случайный элемент
            _audio.PlayFromGroup(collectSoundGroupId, loop: false, pitch: 1f, speed: 1f);
        }
        else
        {
            // конкретный элемент по индексу
            var sound = _library.GetFromGroupByIndex(collectSoundGroupId, soundIndex);
            if (sound != null)
                _audio.PlaySound(sound, loop: false, pitch: 1f, speed: 1f, groupId: collectSoundGroupId);
        }
    }

    protected virtual void ApplyEffect()
    {
        // Базовая реализация пустая
    }
}