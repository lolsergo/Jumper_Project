using UnityEngine;
using MVVM;

public sealed class AudioTriggerView : MonoBehaviour
{
    [Data("PlaySound")] // ����� � ViewModel!
    public string SoundID; // ������������� � ����������
}
