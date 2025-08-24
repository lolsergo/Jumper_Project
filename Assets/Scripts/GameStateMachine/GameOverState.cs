using UnityEngine;

public class GameOverState : GameState
{
    private readonly GameObject _resultsScreen;
    private readonly IEventBus _bus;

    public GameOverState(GameObject resultsScreen, IEventBus bus)
        : base()
    {
        _resultsScreen = resultsScreen;
        _bus = bus;
    }

    public override void Enter()
    {
        Time.timeScale = 0f;
        _resultsScreen.SetActive(true);
        _bus.Publish(new GameOverEvent());
        Debug.Log("Game over");
    }

    public override void Exit()
    {
        _resultsScreen.SetActive(false);
    }
}
