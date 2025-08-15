using UnityEngine;
using Zenject;

public class PlayerCurrency : MonoBehaviour
{
    [SerializeField]
    private int _gold;
    public int Gold { get => _gold; private set => _gold = value; }

    public event System.Action<int> OnGoldChanged;
    public void IncreaseGold(int value)
    {
        Gold += value;
        Debug.Log($"Current gold: {Gold}");
        OnGoldChanged?.Invoke(value);
    }
}
