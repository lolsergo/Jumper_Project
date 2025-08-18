using UnityEngine;

public class GameplayState : GameState
{
    public GameplayState(GameStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        Time.timeScale = 1f;
    }
}
