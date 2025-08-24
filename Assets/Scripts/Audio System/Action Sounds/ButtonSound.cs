using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Button))]
public class ButtonSound : MonoBehaviour
{
    [SerializeField]
    private SoundID _soundID = SoundID.UIButtonClick;

    private Button _button;

    [Inject] private AudioProvider _audioManager;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(PlaySound);
    }

    private void PlaySound()
    {
        if (_audioManager == null)
        {
            Debug.LogWarning("[ButtonSound] AudioManager == null. Make sure this component is injected by Zenject.");
            return;
        }

        _audioManager.Play(_soundID);
    }

    private void OnDestroy()
    {
        if (_button != null)
            _button.onClick.RemoveListener(PlaySound);
    }
}