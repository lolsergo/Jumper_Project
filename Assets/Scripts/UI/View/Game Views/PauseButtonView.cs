using UnityEngine;
using UnityEngine.UI;
using MVVM;

public sealed class PauseButtonView : MonoBehaviour
{
    [Data("PauseClick")]
    public Button Button;  // Перетяни сюда кнопку в инспекторе
}
