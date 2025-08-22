using MVVM;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyHealthButtonView : MonoBehaviour
{
    [Data("BuyHealthClick")]  // Привязывается к BuyHealthAction во ViewModel
    public Button Button;      // Кнопка для покупки здоровья

    [Data("ButtonColor")]     // Привязывается к ButtonColor во ViewModel
    public Image ButtonImage;  // Image, который меняет цвет (зелёный/серый)

    [Data("Price")]  // привязка к ReactiveProperty<int> во ViewModel
    public TMP_Text PriceText;
}
