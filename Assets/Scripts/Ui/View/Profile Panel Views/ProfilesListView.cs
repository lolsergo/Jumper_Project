using UnityEngine;
using MVVM;

public class ProfilesListView : MonoBehaviour
{
    [Data("ProfilesContainer")]
    public Transform Container;

    // prefab ������
    [Data("ProfileButtonPrefab")]
    public ProfileButtonView ButtonPrefab;
}