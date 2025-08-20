using UnityEngine;
using UnityEngine.UI;
using Zenject;

public sealed class ProfilesSceneController : MonoBehaviour
{
    [SerializeField]
    private GameObject createProfilePanel;   // Панель с полем ввода
    [SerializeField]
    private TMPro.TMP_InputField nameInput;  // Поле для ввода имени
    [SerializeField] 
    private Button _openCreatePanelButton;  // Поле для ввода имени

    private IUserProfileService _profileService;

    [Inject]
    public void Construct(IUserProfileService profileService)
    {
        _profileService = profileService;
    }

    private void Awake()
    {
        createProfilePanel.SetActive(false);
    }

    /// <summary>
    /// Открывает панель создания нового профиля.
    /// </summary>
    public void OpenCreateProfilePanel()
    {
        nameInput.text = string.Empty; // очищаем старое значение
        createProfilePanel.SetActive(true);
        _openCreatePanelButton.gameObject.SetActive(false);
    }

    /// <summary>
    /// Закрывает панель (без создания).
    /// </summary>
    public void CloseCreateProfilePanel()
    {
        createProfilePanel.SetActive(false);
        _openCreatePanelButton.gameObject.SetActive(true);
    }

    /// <summary>
    /// Подтверждение создания профиля.
    /// </summary>
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