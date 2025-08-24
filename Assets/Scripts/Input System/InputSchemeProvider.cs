public class InputSchemeProvider
{
    private readonly InputController _input;

    public InputSchemeProvider(InputController input, GameStateMachine stateMachine)
    {
        _input = input;
        stateMachine.OnStateChanged += HandleStateChanged;
    }

    private void HandleStateChanged(GameState oldState, GameState newState)
    {
        switch (newState)
        {
            case GameplayState:
                SwitchScheme("Gameplay");
                break;
            case PausedState:
                SwitchScheme("UI");
                break;
            case GameOverState:
                SwitchScheme("UI");
                break;
        }
    }

    private void SwitchScheme(string mapName)
    {
        var playerInput = _input.GetComponent<UnityEngine.InputSystem.PlayerInput>();
        playerInput?.SwitchCurrentActionMap(mapName);
    }
}
