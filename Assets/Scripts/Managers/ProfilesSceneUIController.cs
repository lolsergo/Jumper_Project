using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

public sealed class ProfilesSceneUIController : MonoBehaviour
{
    [Header("Create Panel")]
    [SerializeField] private GameObject createProfilePanel;
    [SerializeField] private GameObject _availableProfiles;
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private Button _openCreatePanelButton;

    [Header("Delete Panel")]
    [SerializeField] private GameObject _acceptDeleteProfilePanel;
    [SerializeField] private TMP_Text _deleteProfileNameText;

    private IProfilesSceneLogic _logic;

    [Inject]
    public void Construct(IProfilesSceneLogic logic)
    {
        _logic = logic;
    }

    private void Awake()
    {
        if (createProfilePanel != null) createProfilePanel.SetActive(false);
        if (_acceptDeleteProfilePanel != null) _acceptDeleteProfilePanel.SetActive(false);
    }

    // ==== Методы с теми же именами, что были в ProfilesSceneController ====

    public void OpenCreateProfilePanel()
    {
        if (nameInput != null) nameInput.text = string.Empty;
        if (createProfilePanel != null) createProfilePanel.SetActive(true);
        if (_openCreatePanelButton != null) _openCreatePanelButton.gameObject.SetActive(false);
        if (_availableProfiles != null) _availableProfiles.SetActive(false);
    }

    public void CloseCreateProfilePanel()
    {
        if (createProfilePanel != null) createProfilePanel.SetActive(false);
        if (_openCreatePanelButton != null) _openCreatePanelButton.gameObject.SetActive(true);
        if (_availableProfiles != null) _availableProfiles.SetActive(true);
    }

    public void OpenAcceptDeleteProfilePanel(string profileName)
    {
        _logic.SetSelectedProfile(profileName);
        if (_deleteProfileNameText != null)
            _deleteProfileNameText.text = profileName;
        if (_acceptDeleteProfilePanel != null)
            _acceptDeleteProfilePanel.SetActive(true);
    }

    public void CloseAcceptDeleteProfilePanel()
    {
        if (_acceptDeleteProfilePanel != null)
            _acceptDeleteProfilePanel.SetActive(false);
        _logic.SetSelectedProfile(null);
        if (_deleteProfileNameText != null)
            _deleteProfileNameText.text = string.Empty;
    }

    public void ConfirmDeleteSelectedProfile()
    {
        if (_logic.TryDeleteSelectedProfile(out var deleted, out var error))
        {
            if (!string.IsNullOrEmpty(deleted))
                Debug.Log($"Профиль '{deleted}' удалён.");
        }
        else if (!string.IsNullOrEmpty(error))
        {
            Debug.LogWarning(error);
        }

        CloseAcceptDeleteProfilePanel();
    }

    public void ConfirmCreateProfile(string profileName)
    {
        if (_logic.TryCreateProfile(profileName, out var error))
        {
            Debug.Log($"Профиль '{profileName.Trim()}' создан.");
            CloseCreateProfilePanel();
        }
        else
        {
            Debug.LogWarning(error);
            // Панель создания оставляем открытой, чтобы пользователь мог исправить
        }
    }
}