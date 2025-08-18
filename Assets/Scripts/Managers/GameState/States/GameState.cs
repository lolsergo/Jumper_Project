public abstract class GameState
{
    protected GameStateMachine StateMachine;

    protected GameState(GameStateMachine stateMachine)
    {
        StateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
