using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class Bootstrap : MonoBehaviour
{
    private UserProfileService _profileManager;

    [Inject]
    public void Construct(UserProfileService profileManager)
    {
        _profileManager = profileManager;
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("ActiveProfile"))
        {
            string profileName = PlayerPrefs.GetString("ActiveProfile");
            _profileManager.LoadProfile(profileName);
            SceneManager.LoadScene("SceneMainMenu");
        }
        else
        {
            SceneManager.LoadScene("SceneProfileSelect");
        }
    }
}