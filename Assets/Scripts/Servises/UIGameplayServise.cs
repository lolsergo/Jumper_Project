using UnityEngine;
using UnityEngine.UI;

public class UIGameServise : MonoBehaviour
{
    [SerializeField] private GameObject _pauseScreen;
    [SerializeField] private GameObject _loseScreen;
    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private Button _pauseButton;
    public GameObject SettingsPanel => _settingsPanel;

    private void Awake()
    {
        _pauseScreen.SetActive(false);
        _loseScreen.SetActive(false);
        _gameOverScreen.SetActive(false);
        _settingsPanel.SetActive(false);
    }

    public void ShowPauseScreen() => _pauseScreen.SetActive(true);
    public void HidePauseScreen() => _pauseScreen.SetActive(false);

    public void ShowSettings() => _settingsPanel.SetActive(true);
    public void HideSettings() => _settingsPanel.SetActive(false);

    public void ShowLoseScreen() => _loseScreen.SetActive(true);
    public void HideLoseScreen() => _loseScreen.SetActive(false);

    public void ShowGameOverScreen() => _gameOverScreen.SetActive(true);
    public void ShowPauseButton() => _pauseButton.gameObject.SetActive(true);
    public void HidePauseButton() => _pauseButton.gameObject.SetActive(false);
}