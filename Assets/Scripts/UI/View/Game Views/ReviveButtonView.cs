using UnityEngine;
using UnityEngine.UI;
using MVVM;

public sealed class ReviveButtonView : MonoBehaviour
{
    // Для клика (ButtonBinder свяжет с Revive)
    [Data("ReviveButton")]
    public Button Button;

    // Для видимости (GameObjectActiveBinder свяжет с IsVisible)
    // Можно указать сам объект кнопки либо родительский контейнер.
    [Data("ReviveButtonVisible")]
    public GameObject Root;
}
