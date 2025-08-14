using UnityEngine;

public class GameSpeedManager : MonoBehaviour
{
    [SerializeField]
    private float _gameSpeed;
    private float _baseGameSpeed;
    public float GameSpeed { get => _gameSpeed; set => _gameSpeed = value; }

    [SerializeField]
    private float increaseGameSpeedMultiplier;
    [SerializeField]
    private float _basetimeBetweenSpeedIncrease;
    private float _timeBetweenSpeedIncrease;

    private void Awake()
    {
        _baseGameSpeed = _gameSpeed;
        _timeBetweenSpeedIncrease = _basetimeBetweenSpeedIncrease;
    }

    private void Update()
    {
        _timeBetweenSpeedIncrease -= Time.deltaTime;

        if (_timeBetweenSpeedIncrease <= 0)
        {
            IncreaseGameSpeed();
            _timeBetweenSpeedIncrease = _basetimeBetweenSpeedIncrease;
        }
    }

    public void IncreaseGameSpeed()
    {
        GameSpeed += (_baseGameSpeed * (increaseGameSpeedMultiplier / 100 + 1));
    }

    public void DecreaseGameSpeed()
    {
        GameSpeed /= 2;
        if (GameSpeed < _baseGameSpeed)
        {
            GameSpeed = _baseGameSpeed;
        }
    }
}
