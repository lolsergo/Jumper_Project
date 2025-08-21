using UnityEngine;
using UnityEngine.UI;

public class ProfileUIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Transform profilesContainer; // Контейнер для кнопок
    [SerializeField] private GameObject profileButtonPrefab;
    [SerializeField] private InputField newProfileInput;
    [SerializeField] private Button createProfileButton;
    [SerializeField] private Button refreshButton;

    [Header("Managers")]
    [SerializeField] private UserProfileService profileManager;

    private void Start()
    {
        createProfileButton.onClick.AddListener(CreateProfile);
        refreshButton.onClick.AddListener(RefreshProfiles);

        RefreshProfiles();
    }

    private void CreateProfile()
    {
        string profileName = newProfileInput.text.Trim();
        if (!string.IsNullOrEmpty(profileName))
        {
            profileManager.CreateProfile(profileName);
            RefreshProfiles();
            newProfileInput.text = "";
        }
    }

    private void RefreshProfiles()
    {
        // Удаляем старые кнопки
        foreach (Transform child in profilesContainer)
            Destroy(child.gameObject);

        // Получаем список профилей
        var profiles = SaveSystem.GetAllProfiles();

        foreach (var profile in profiles)
        {
            var buttonGO = Instantiate(profileButtonPrefab, profilesContainer);
            var btnText = buttonGO.GetComponentInChildren<Text>();
            btnText.text = profile;

            buttonGO.GetComponent<Button>().onClick.AddListener(() =>
            {
                profileManager.LoadProfile(profile);
                Debug.Log($"Загружен профиль: {profile}");
            });
        }
    }
}