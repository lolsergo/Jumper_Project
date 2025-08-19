using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Button))]
public class ButtonSoundPlayer : MonoBehaviour
{
    [SerializeField]
    private SoundID _soundID = SoundID.UIButtonClick;

    private Button _button;
    private AudioManager _audioManager;

    [Inject]
    public void Construct(AudioManager audioManager)
    {
        _audioManager = audioManager;
    }

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(PlaySound);
    }

    private void PlaySound()
    {
        _audioManager.Play(_soundID);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(PlaySound);
    }
}