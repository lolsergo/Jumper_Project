using UnityEngine;

public class PausedState : GameState
{
    private readonly GameObject _pauseScreen;
    private readonly IEventBus _bus;

    public PausedState(GameObject pauseScreen, IEventBus bus)
        : base()
    {
        _pauseScreen = pauseScreen;
        _bus = bus;
    }

    public override void Enter()
    {
        Time.timeScale = 0f;
        _pauseScreen.SetActive(true);
        _bus.Publish(new GamePausedEvent());
    }

    public override void Exit()
    {
        Time.timeScale = 1f;
        _pauseScreen.SetActive(false);
        _bus.Publish(new GameResumedEvent());
    }
}
