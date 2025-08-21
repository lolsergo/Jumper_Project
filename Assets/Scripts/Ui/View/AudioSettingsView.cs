using UnityEngine;
using UnityEngine.UI;
using MVVM;

public sealed class AudioSettingsView : MonoBehaviour
{
    [Data("SFX")]
    public Slider sliderSFX;

    [Data("Music")]
    public Slider sliderMusic;

    [Data("UI")]
    public Slider sliderUI;
}
