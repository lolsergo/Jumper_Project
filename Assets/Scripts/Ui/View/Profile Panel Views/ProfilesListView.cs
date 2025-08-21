using UnityEngine;
using MVVM;
using UnityEngine.UI;

public class ProfilesListView : MonoBehaviour
{
    [Data("ProfilesContainer")]
    public Transform Container;

    [Data("ProfileButtonPrefab")]
    public ProfileButtonView ButtonPrefab;

    [Data("CreateProfile")]
    public Button CreateProfileButton;

    [Data("ToggleDeleteMode")]
    public Button ToggleDeleteModeButton;
}