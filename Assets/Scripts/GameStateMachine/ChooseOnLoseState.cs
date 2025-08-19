using UnityEngine;

public class ChooseOnLoseState : GameState
{
    private readonly GameObject _chooseScreen;

    public ChooseOnLoseState(GameStateMachine stateMachine, GameObject chooseScreen)
        : base(stateMachine)
    {
        _chooseScreen = chooseScreen;
        _chooseScreen.SetActive(true);
        Time.timeScale = 0f; // Главная строка для паузы
    }

    public override void Exit()
    {
        _chooseScreen.SetActive(false);
        Time.timeScale = 1f;
    }
}
