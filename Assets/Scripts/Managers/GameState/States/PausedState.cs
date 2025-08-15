using UnityEngine;

public class PausedState : GameState
{
    private readonly GameObject _pauseScreen;

    public PausedState(GameStateMachine stateMachine, GameObject pauseScreen)
        : base(stateMachine)
    {
        _pauseScreen = pauseScreen;
    }

    public override void Enter()
    {
        Time.timeScale = 0f;
        _pauseScreen.SetActive(true);
        Debug.Log("Game paused");
    }

    public override void Exit()
    {
        _pauseScreen.SetActive(false);
    }
}
