using System;

public class PlayerProvider
{
    private PlayerHealth _playerHealth;
    public PlayerHealth PlayerHealth => _playerHealth;

    public event Action<PlayerHealth> PlayerSet;

    public void SetPlayer(PlayerHealth health)
    {
        _playerHealth = health;
        PlayerSet?.Invoke(_playerHealth);
    }

    public void Clear()
    {
        _playerHealth = null;
        PlayerSet?.Invoke(null);
    }
}
