using UnityEngine;
using UnityEngine.UI;
using MVVM;
using TMPro;

public class ProfileButtonView : MonoBehaviour
{
    [Data("Label")]
    public TMP_Text Label;

    [Data("OnClick")]
    public Button Button;
}