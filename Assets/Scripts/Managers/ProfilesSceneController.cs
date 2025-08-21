using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

public sealed class ProfilesSceneController : MonoBehaviour
{
    [SerializeField] private GameObject createProfilePanel;
    [SerializeField] private GameObject _availableProfiles;
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private Button _openCreatePanelButton;

    [Header("Accept Delete Panel")]
    [SerializeField] private GameObject _acceptDeleteProfilePanel;
    [SerializeField] private TMP_Text _deleteProfileNameText;

    private IUserProfileService _profileService;
    private string _selectedProfileName;

    [Inject]
    public void Construct(IUserProfileService profileService)
    {
        _profileService = profileService;
    }

    private void Awake()
    {
        createProfilePanel.SetActive(false);
        _acceptDeleteProfilePanel.SetActive(false);
    }

    public void OpenCreateProfilePanel()
    {
        nameInput.text = string.Empty;
        createProfilePanel.SetActive(true);
        _openCreatePanelButton.gameObject.SetActive(false);
        _availableProfiles.SetActive(false);
    }

    public void CloseCreateProfilePanel()
    {
        createProfilePanel.SetActive(false);
        _openCreatePanelButton.gameObject.SetActive(true);
        _availableProfiles.SetActive(true);
    }

    public void OpenAcceptDeleteProfilePanel(string profileName)
    {
        _selectedProfileName = profileName;
        if (_deleteProfileNameText != null)
            _deleteProfileNameText.text = profileName;
        _acceptDeleteProfilePanel.SetActive(true);
    }

    public void CloseAcceptDeleteProfilePanel()
    {
        _acceptDeleteProfilePanel.SetActive(false);
        _selectedProfileName = null;
    }

    public void ConfirmDeleteSelectedProfile()
    {
        if (!string.IsNullOrEmpty(_selectedProfileName))
        {
            _profileService.DeleteProfile(_selectedProfileName);
            Debug.Log($"Профиль '{_selectedProfileName}' удалён.");
        }
        CloseAcceptDeleteProfilePanel();
    }

    public void ConfirmCreateProfile(string profileName)
    {
        var name = profileName.Trim();
        if (string.IsNullOrEmpty(name))
        {
            Debug.LogWarning("Имя профиля не может быть пустым.");
            return;
        }
        try
        {
            _profileService.CreateProfile(name);
            Debug.Log($"Профиль '{name}' создан.");
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning($"Ошибка создания профиля: {ex.Message}");
        }
        CloseCreateProfilePanel();
    }
}