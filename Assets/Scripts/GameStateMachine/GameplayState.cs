using UnityEngine;

public class GameplayState : GameState
{
    private readonly IEventBus _bus;

    public GameplayState(IEventBus bus) : base()
    {
        _bus = bus;
    }

    public override void Enter()
    {
        Time.timeScale = 1f;
        _bus.Publish(new GameplayStartedEvent());
    }
}