using UnityEngine;
using MVVM;
using TMPro; // если TextMeshPro
using UniRx;

public class NewProfileNameInputView : MonoBehaviour
{
    [Data("NewProfileNameInput")]
    public TMP_InputField InputField;
}
