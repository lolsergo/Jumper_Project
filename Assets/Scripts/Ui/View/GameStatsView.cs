using TMPro;
using UnityEngine;
using MVVM;

public class GameStatsView : MonoBehaviour
{
    [Data("MaxDistance")]
    public TMP_Text maxDistanceText;
    [Data("Tries")]
    public TMP_Text triesText;
    [Data("PlayTime")]
    public TMP_Text playTimeText;
}
