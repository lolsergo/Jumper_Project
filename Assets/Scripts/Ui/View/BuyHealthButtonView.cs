using UnityEngine.UI;
using MVVM;
using UnityEngine;

public class BuyHealthButtonView : MonoBehaviour
{
    [Data("BuyHealthClick")]  // ������������� � BuyHealthAction �� ViewModel
    public Button Button;      // ������ ��� ������� ��������

    [Data("ButtonColor")]     // ������������� � ButtonColor �� ViewModel
    public Image ButtonImage;  // Image, ������� ������ ���� (������/�����)
}
