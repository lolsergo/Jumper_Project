using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/PriceConfig")]
public class PriceConfigSO : ScriptableObject
{
    [Min(1)]
    public int basePrice = 5;         // начальная цена
    [Min(1f)]
    public float multiplier = 2f;     // множитель
}
