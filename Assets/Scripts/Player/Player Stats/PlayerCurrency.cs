using UniRx;
using UnityEngine;
using Zenject;

public class PlayerCurrency : MonoBehaviour
{
    [SerializeField] private int _initialGold = 0;
    private readonly ReactiveProperty<int> _gold = new ReactiveProperty<int>();

    public IReadOnlyReactiveProperty<int> Gold => _gold;

    [Inject]
    private void Construct()
    {
        _gold.Value = _initialGold;
    }

    public void IncreaseGold(int value)
    {
        _gold.Value += value;
    }

    public void DeacreaseGold(in int value)
    {
        _gold.Value -= value;
    }
}
