public abstract class GameState
{
    protected GameState() { }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
