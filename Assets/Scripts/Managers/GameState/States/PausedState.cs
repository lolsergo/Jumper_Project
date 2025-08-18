using UnityEngine;

public class PausedState : GameState
{
    private readonly GameObject _pauseScreen;

    public PausedState(GameStateMachine stateMachine, GameObject pauseScreen)
        : base(stateMachine)
    {
        _pauseScreen = pauseScreen;
        _pauseScreen.SetActive(true);
        Time.timeScale = 0f; // Главная строка для паузы
    }

    public override void Exit()
    {
        _pauseScreen.SetActive(false);
        Time.timeScale = 1f;
    }
}
