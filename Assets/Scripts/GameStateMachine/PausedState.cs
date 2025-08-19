using UnityEngine;
using UniRx;

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
        GameEvents.OnGamePaused.OnNext(Unit.Default);
    }

    public override void Exit()
    {
        Time.timeScale = 1f;
        _pauseScreen.SetActive(false);
        GameEvents.OnGameResumed.OnNext(Unit.Default);
    }
}
