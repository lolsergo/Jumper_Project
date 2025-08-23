using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Button))]
public class ButtonSound : MonoBehaviour
{
    [SerializeField]
    private SoundID _soundID = SoundID.UIButtonClick;

    private Button _button;

    [Inject] private AudioManager _audioManager;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(PlaySound);
    }

    private void PlaySound()
    {
        if (_audioManager == null)
        {
            Debug.LogWarning("[ButtonSound] AudioManager == null");
            TryLateResolve();
            if (_audioManager == null) return;
        }

        _audioManager.Play(_soundID);
    }

    private void TryLateResolve()
    {
        var ctx = ProjectContext.Instance != null
            ? ProjectContext.Instance.Container
            : null;

        if (ctx == null)
        {
            var sceneContext = UnityEngine.Object.FindFirstObjectByType<Zenject.SceneContext>();
            if (sceneContext != null)
            {
                ctx = sceneContext.Container;
            }
        }

        if (ctx != null && _audioManager == null)
        {
            if (ctx.HasBinding<AudioManager>())
                _audioManager = ctx.Resolve<AudioManager>();
        }
    }

    private void OnDestroy()
    {
        if (_button != null)
            _button.onClick.RemoveListener(PlaySound);
    }
}