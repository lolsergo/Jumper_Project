using UnityEngine;

public class PriceService
{
    private readonly PriceConfigSO _config;
    private int _purchaseCount;

    public PriceService(PriceConfigSO config)
    {
        _config = config;
        _purchaseCount = 0;
    }

    public int CurrentPrice => Mathf.RoundToInt(
        _config.basePrice * Mathf.Pow(_config.multiplier, _purchaseCount)
    );

    public void RegisterPurchase()
    {
        _purchaseCount++;
    }
}
