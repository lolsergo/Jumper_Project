using UnityEngine.UI;
using MVVM;
using UnityEngine;

public class BuyHealthButtonView : MonoBehaviour
{
    [Data("BuyHealthClick")]  // Привязывается к BuyHealthAction во ViewModel
    public Button Button;      // Кнопка для покупки здоровья

    [Data("ButtonColor")]     // Привязывается к ButtonColor во ViewModel
    public Image ButtonImage;  // Image, который меняет цвет (зелёный/серый)
}
