using System;
using UniRx;
using UnityEngine;
using Zenject;

public class PlayerCurrency : MonoBehaviour
{
    [SerializeField] private int _initialGold = 0;
    private readonly ReactiveProperty<int> _gold = new ReactiveProperty<int>();

    // Для совместимости со старым кодом оставляем событие
    public event Action<int> OnGoldChanged;

    public IReadOnlyReactiveProperty<int> Gold => _gold;

    [Inject]
    private void Construct()
    {
        _gold.Value = _initialGold;
        _gold.Subscribe(gold => OnGoldChanged?.Invoke(gold));
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
