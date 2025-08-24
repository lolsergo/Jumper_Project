using UnityEngine;

public class ChooseOnLoseState : GameState
{
    private readonly GameObject _chooseScreen;
    private readonly IEventBus _bus;

    public ChooseOnLoseState(GameObject chooseScreen, IEventBus bus)
        : base()
    {
        _chooseScreen = chooseScreen;
        _chooseScreen.SetActive(true);
        Time.timeScale = 0f;
        _bus = bus;
        _bus.Publish(new GamePausedEvent());
    }

    public override void Exit()
    {
        _chooseScreen.SetActive(false);
        Time.timeScale = 1f;
        _bus.Publish(new GameResumedEvent());
    }
}
