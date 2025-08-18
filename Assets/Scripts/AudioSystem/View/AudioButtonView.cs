using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AudioButtonView : MonoBehaviour
{
    [SerializeField] private string _soundID = "UI.Click";

    public Button Button => GetComponent<Button>();
    public string SoundID => _soundID;
}