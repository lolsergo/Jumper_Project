using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Gameplay,
        Paused,
        GameOver,
    }

    public GameState currentState;
    public GameState previousState;

    [Header("Screens")]
    public GameObject pauseScreen;
    public GameObject resultsScreen;

    [Header("Current Stat Displays")]
    public TMP_Text currentHealthDisplay;
    public TMP_Text currentGameSpeedDisplay;
    public TMP_Text currentGoldDisplay;
    public TMP_Text currentMagnetDisplay;

    [Header("Result Screen Displays")]
    public TMP_Text distanceReachedDisplay;
    public bool isGameOver = false;

    private InputController _inputController;

    [Inject]
    private void Construct(InputController inputController)
    {
        _inputController = inputController;
    }

    private void Awake()
    {
        DisableScreens();
    }

    private void OnEnable()
    {
        if (_inputController != null)
        {
            var pauseAction = _inputController.actions.Find(a => a.name == "Pause");
            if (pauseAction != null)
            {
                pauseAction.OnPressed += CheckForPauseAndResume;
            }
        }
    }

    private void OnDisable()
    {
        if (_inputController != null)
        {
            var pauseAction = _inputController.actions.Find(a => a.name == "Pause");
            if (pauseAction != null)
            {
                pauseAction.OnPressed += CheckForPauseAndResume;
            }
        }
    }

    private void Update()
    {
        switch (currentState)
        {
            case GameState.Gameplay:
                break;
            case GameState.Paused:
                break;
            case GameState.GameOver:
                if (!isGameOver)
                {
                    isGameOver = true;
                    Time.timeScale = 0f;
                    Debug.Log("Game over");
                    DisplayResults();
                }
                break;
            default:
                Debug.LogWarning("State doesn't exist");
                break;
        }
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
    }

    public void PauseGame()
    {
        if (currentState != GameState.Paused)
        {
            previousState = currentState;
            ChangeState(GameState.Paused);
            Time.timeScale = 0f;
            pauseScreen.SetActive(true);
            Debug.Log("Game is paused");
        }
    }

    public void ResumeGame()
    {
        if (currentState == GameState.Paused)
        {
            ChangeState(previousState);
            Time.timeScale = 1f;
            pauseScreen.SetActive(false);
            Debug.Log("Game is resumed");
        }
    }

    private void CheckForPauseAndResume()
    {
        if (currentState == GameState.Paused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    private void DisableScreens()
    {
        pauseScreen.SetActive(false);
        resultsScreen.SetActive(false);
    }

    public void GameOver()
    {
        ChangeState(GameState.GameOver);
    }

    private void DisplayResults()
    {
        resultsScreen.SetActive(true);
    }

    public void AssignDistanceReachedUI(int levelReachedData)
    {
        distanceReachedDisplay.text = levelReachedData.ToString();
    }

    public void ReturnToTitleScreen()
    {

        Time.timeScale = 1f;

        SceneManager.LoadScene("Title Screen");
    }
}

