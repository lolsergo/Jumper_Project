using UnityEngine;
using UnityEngine.UI;
using MVVM;

public sealed class ResumeButtonView : MonoBehaviour
{
    [Data("ResumeClick")]
    public Button Button;  // Перетяни сюда кнопку в инспекторе
}
