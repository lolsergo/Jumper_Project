using UnityEngine;
using UnityEngine.UI;
using MVVM;

public sealed class MenuButtonView : MonoBehaviour
{
    [Data("MenuClick")]
    public Button Button;
}
