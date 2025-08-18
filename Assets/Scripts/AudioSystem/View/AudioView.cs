using UnityEngine;
using MVVM;

public sealed class AudioTriggerView : MonoBehaviour
{
    [Data("PlaySound")] // Связь с ViewModel!
    public string SoundID; // Настраивается в инспекторе
}
