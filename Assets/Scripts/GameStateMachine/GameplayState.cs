using UniRx;
using UnityEngine;

public class GameplayState : GameState
{
    private readonly IEventBus _bus;

    public GameplayState(GameStateMachine stateMachine, IEventBus bus) : base(stateMachine)
    {
        _bus = bus;
    }

    public override void Enter()
    {
        Time.timeScale = 1f;
        _bus.Publish(new GameplayStartedEvent());
    }
}