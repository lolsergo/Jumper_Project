using MVVM;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyHealthButtonView : MonoBehaviour
{
    [Data("BuyHealthClick")]  // ������������� � BuyHealthAction �� ViewModel
    public Button Button;      // ������ ��� ������� ��������

    [Data("ButtonColor")]     // ������������� � ButtonColor �� ViewModel
    public Image ButtonImage;  // Image, ������� ������ ���� (������/�����)

    [Data("Price")]  // �������� � ReactiveProperty<int> �� ViewModel
    public TMP_Text PriceText;
}
