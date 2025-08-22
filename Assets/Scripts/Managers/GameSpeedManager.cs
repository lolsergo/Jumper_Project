using UnityEngine;
using UniRx;
using System;

public class GameSpeedManager : MonoBehaviour
{
    [SerializeField] private float _gameSpeed;
    private float _baseGameSpeed;
    public float GameSpeed { get => _gameSpeed; set => _gameSpeed = value; }

    [SerializeField] private float increaseGameSpeedMultiplier;
    [SerializeField] private float _basetimeBetweenSpeedIncrease;
    private float _timeBetweenSpeedIncrease;

    private readonly ReactiveProperty<float> _distance = new (0f);
    public IObservable<float> DistanceReached => _distance;
    public float CurrentDistance => _distance.Value;

    private void Awake()
    {
        _baseGameSpeed = _gameSpeed;
        _timeBetweenSpeedIncrease = _basetimeBetweenSpeedIncrease;
    }

    private void Update()
    {
        _distance.Value += _gameSpeed * Time.deltaTime;

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
            GameSpeed = _baseGameSpeed;
    }

    public void ResetSpeed()
    {
        _gameSpeed = _baseGameSpeed;
        _timeBetweenSpeedIncrease = _basetimeBetweenSpeedIncrease;
        _distance.Value = 0f;
    }
}
