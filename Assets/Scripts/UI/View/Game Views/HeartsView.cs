using UnityEngine;
using UnityEngine.UI;
using MVVM;

public sealed class HeartsView : MonoBehaviour
{
    [Data("Health")]
    public Image[] _hearts;
}