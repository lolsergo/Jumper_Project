using UnityEngine;
using UnityEngine.UI;
using Zenject;

public sealed class ProfilesSceneController : MonoBehaviour
{
    [SerializeField]
    private GameObject createProfilePanel;   // ������ � ����� �����
    [SerializeField]
    private TMPro.TMP_InputField nameInput;  // ���� ��� ����� �����
    [SerializeField] 
    private Button _openCreatePanelButton;  // ���� ��� ����� �����

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
    /// ��������� ������ �������� ������ �������.
    /// </summary>
    public void OpenCreateProfilePanel()
    {
        nameInput.text = string.Empty; // ������� ������ ��������
        createProfilePanel.SetActive(true);
        _openCreatePanelButton.gameObject.SetActive(false);
    }

    /// <summary>
    /// ��������� ������ (��� ��������).
    /// </summary>
    public void CloseCreateProfilePanel()
    {
        createProfilePanel.SetActive(false);
        _openCreatePanelButton.gameObject.SetActive(true);
    }

    /// <summary>
    /// ������������� �������� �������.
    /// </summary>
    public void ConfirmCreateProfile(string profileName)
    {
        var name = profileName.Trim();
        if (string.IsNullOrEmpty(name))
        {
            Debug.LogWarning("��� ������� �� ����� ���� ������.");
            return;
        }
        try
        {
            _profileService.CreateProfile(name);
            Debug.Log($"������� '{name}' ������.");
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning($"������ �������� �������: {ex.Message}");
        }
        CloseCreateProfilePanel();
    }
}