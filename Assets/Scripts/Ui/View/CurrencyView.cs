using UnityEngine;
using TMPro;
using MVVM;

public sealed class CurrencyView : MonoBehaviour
{
    [Data("Gold")]
    public TMP_Text currencyText;
}
