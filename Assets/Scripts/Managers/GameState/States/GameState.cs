public abstract class GameState
{
    protected GameStateMachine StateMachine;

    public GameState(GameStateMachine stateMachine)
    {
        StateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Update() { }
}
