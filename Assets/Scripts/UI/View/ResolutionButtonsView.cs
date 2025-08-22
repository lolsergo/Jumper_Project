using UnityEngine.UI;
using MVVM;
using UnityEngine;
using TMPro;

public class ResolutionButtonsView : MonoBehaviour
{
    [Data("SetResolution1")]
    public Button ButtonResolution1;
    [Data("SetResolution2")]
    public Button ButtonResolution2;
    [Data("SetResolution3")]
    public Button ButtonResolution3;

    [Data("Resolution1Text")]
    public TMP_Text Resolution1Text;
    [Data("Resolution2Text")]
    public TMP_Text Resolution2Text;
    [Data("Resolution3Text")]
    public TMP_Text Resolution3Text;
}
