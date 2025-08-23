using UnityEngine;

public class UIGameManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject SettingsPanel;

    private void Awake()
    {
        pauseScreen.SetActive(false);
        loseScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        SettingsPanel.SetActive(false);
    }

    public void ShowPauseScreen() => pauseScreen.SetActive(true);
    public void HidePauseScreen() => pauseScreen.SetActive(false);

    public void ShowSettings() => SettingsPanel.SetActive(true);
    public void HideSettings() => SettingsPanel.SetActive(false);

    public void ShowLoseScreen() => loseScreen.SetActive(true);
    public void HideLoseScreen() => loseScreen.SetActive(false);

    public void ShowGameOverScreen() => gameOverScreen.SetActive(true);
}