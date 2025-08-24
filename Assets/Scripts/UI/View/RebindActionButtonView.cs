using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MVVM;

public class RebindActionButtonView : MonoBehaviour
{
    [Data("StartRebind")]
    public Button RebindButton;

    [Data("ResetBinding")]
    public Button ResetButton;

    [Data("DisplayName")]
    public TMP_Text BindingLabel;

    [Data("IsRebinding")]
    public GameObject RebindingIndicator;

    [Data("CommandName")]
    public TMP_Text CommandLabel;
}
