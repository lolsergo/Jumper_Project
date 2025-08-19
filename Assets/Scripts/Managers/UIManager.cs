using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private GameObject gameOverScreen;

    private void Awake()
    {
        // Выключаем все экраны при старте
        pauseScreen.SetActive(false);
        loseScreen.SetActive(false);
        gameOverScreen.SetActive(false);
    }

    public void ShowPauseScreen() => pauseScreen.SetActive(true);
    public void HidePauseScreen() => pauseScreen.SetActive(false);

    public void ShowLoseScreen() => loseScreen.SetActive(true);
    public void HideLoseScreen() => loseScreen.SetActive(false);
    public void ShowGameOverScreen() => gameOverScreen.SetActive(true);
}