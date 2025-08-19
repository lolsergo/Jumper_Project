using UnityEngine;

public class GameOverState : GameState
{
    private readonly GameObject _resultsScreen;

    public GameOverState(GameStateMachine stateMachine, GameObject resultsScreen)
        : base(stateMachine)
    {
        _resultsScreen = resultsScreen;
    }

    public override void Enter()
    {
        Time.timeScale = 0f;
        _resultsScreen.SetActive(true);
        Debug.Log("Game over");
    }

    public override void Exit()
    {
        _resultsScreen.SetActive(false);
    }
}
