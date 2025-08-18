using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameManager : MonoBehaviour
{
    private GameStateMachine _stateMachine;
    private InputController _inputController;
    private PlayerHealth _playerHealth;

    public GameStateMachine StateMachine => _stateMachine;

    [Header("Screens")]
    public GameObject pauseScreen;
    public GameObject resultsScreen;
    public GameObject chooseOnLoseScreen;

    [Inject]
    private void Construct(InputController inputController, PlayerHealth playerHealth)
    {
        _inputController = inputController;
        _playerHealth = playerHealth;

        // Подписываемся на событие смерти игрока
        _playerHealth.OnDeath.Subscribe(_ => Lose()).AddTo(this);
    }

    private void Awake()
    {
        _stateMachine = new GameStateMachine();
        DisableScreens();
        _stateMachine.ChangeState(new GameplayState(_stateMachine));
    }

    private void OnEnable()
    {
        _inputController.GetAction(InputController.InputActionType.Pause).OnPressed += HandlePauseInput;
    }

    private void OnDisable()
    {
        _inputController.GetAction(InputController.InputActionType.Pause).OnPressed -= HandlePauseInput;
    }

    private void Update()
    {
        _stateMachine.Update();
    }

    public void HandlePauseInput()
    {
        // Если мы в состоянии выбора при проигрыше - игнорируем паузу
        if (_stateMachine.CurrentState is ChooseOnLoseState)
        {
            return;
        }

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

    public void Lose()
    {
        _stateMachine.ChangeState(new ChooseOnLoseState(_stateMachine, chooseOnLoseScreen));
    }

    public void GameOver()
    {
        _stateMachine.ChangeState(new GameOverState(_stateMachine, resultsScreen));
    }

    public void BuyHealth()
    {
        if (_playerHealth.CurrentHealth.Value < _playerHealth.MaxHealth)
        {
            _playerHealth.Heal(_playerHealth.HealthOnRessurect);
            // После покупки здоровья возвращаемся в игровой режим
            ResumeGame();
        }
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    private void DisableScreens()
    {
        pauseScreen.SetActive(false);
        resultsScreen.SetActive(false);
        chooseOnLoseScreen.SetActive(false);
    }
}