using UnityEngine;
using Zenject;

public class Obstacle : LevelObject
{
    [SerializeField] private int _damage = 1;

    private PlayerProvider _playerProvider;

    [SerializeField]
    private SoundGroupID hitSoundGroupId = SoundGroupID.Obstacle_Hit;

    private AudioManager _audio;

    [Inject]
    public void Construct(PlayerProvider playerProvider, AudioManager audio)
    {
        _playerProvider = playerProvider;
        _audio = audio;
    }

    public override void Activate(Vector3 position)
    {
        base.Activate(position);

        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        _collider.enabled = true;
        _renderer.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!gameObject.activeSelf)
            return;

        if (other.CompareTag("Player"))
        {
            var playerHealth = _playerProvider.PlayerHealth;
            if (playerHealth != null && !playerHealth.IsInvincible)
            {
                _audio.PlayFromGroup(hitSoundGroupId, loop: false, pitch: 1f, speed: 1f);
                playerHealth.TakeDamage(_damage);
                _speedManager.DecreaseGameSpeed();
            }
        }
    }
}