using UnityEngine;
using UnityEngine.UI;
using MVVM;

public sealed class ReviveButtonView : MonoBehaviour
{
    // ��� ����� (ButtonBinder ������ � Revive)
    [Data("ReviveButton")]
    public Button Button;

    // ��� ��������� (GameObjectActiveBinder ������ � IsVisible)
    // ����� ������� ��� ������ ������ ���� ������������ ���������.
    [Data("ReviveButtonVisible")]
    public GameObject Root;
}
