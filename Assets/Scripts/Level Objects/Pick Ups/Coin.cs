using UnityEngine;
using Zenject;

public class Coin : Collectibles
{
    [SerializeField]
    private int _goldGranted = 1;
    private PlayerCurrency _playerGold;

    [Inject]
    private void Construct(PlayerCurrency playerGold)
    {
        _playerGold = playerGold;
    }

    protected override void ApplyEffect()
    {
        _playerGold.IncreaseGold(_goldGranted);
    }
}
