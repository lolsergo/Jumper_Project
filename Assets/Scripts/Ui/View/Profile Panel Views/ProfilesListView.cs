using UnityEngine;
using MVVM;

public class ProfilesListView : MonoBehaviour
{
    [Data("ProfilesContainer")]
    public Transform Container;

    // prefab кнопки
    [Data("ProfileButtonPrefab")]
    public ProfileButtonView ButtonPrefab;
}