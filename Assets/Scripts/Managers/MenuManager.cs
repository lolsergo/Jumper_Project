using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _settingsPanel;

    private void Start()
    {
        _settingsPanel.SetActive(false);
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneLoader.Load(SceneType.Game);
    }

    public void OpenSettings()
    {
        _settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        _settingsPanel.SetActive(false);
    }

    public void ChangeProfile()
    {
        SceneLoader.Load(SceneType.Profiles);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Остановка в редакторе
#else
            Application.Quit(); // Выход из собранной версии
#endif
    }
}
