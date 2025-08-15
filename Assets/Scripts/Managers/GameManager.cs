using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameManager : MonoBehaviour
{
    private GameStateMachine _stateMachine;
    private InputController _inputController;

    [Header("Screens")]
    public GameObject pauseScreen;
    public GameObject resultsScreen;

    [Inject]
    private void Construct(InputController inputController)
    {
        _inputController = inputController;
    }

    private void Awake()
    {
        _stateMachine = new GameStateMachine();
        DisableScreens();

        // Начальное состояние
        _stateMachine.ChangeState(new GameplayState(_stateMachine));
    }

    private void OnEnable()
    {
        _inputController.GetAction(InputController.InputActionType.Pause).OnPressed += TogglePause;
    }

    private void OnDisable()
    {
        _inputController.GetAction(InputController.InputActionType.Pause).OnPressed -= TogglePause;
    }

    private void Update()
    {
        _stateMachine.Update();
    }

    private void TogglePause()
    {
        if (_stateMachine.CurrentState is PausedState)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        _stateMachine.ChangeState(new PausedState(_stateMachine, pauseScreen));
    }

    public void ResumeGame()
    {
        _stateMachine.ChangeState(new GameplayState(_stateMachine));
    }

    public void GameOver()
    {
        _stateMachine.ChangeState(new GameOverState(_stateMachine, resultsScreen));
    }

    public void AssignDistanceReachedUI(int levelReachedData)
    {

    }

    public void ReturnToTitleScreen()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Title Screen");
    }

    private void DisableScreens()
    {
        pauseScreen.SetActive(false);
        resultsScreen.SetActive(false);
    }
}