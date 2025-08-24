using System;
using Zenject;

public class SceneInputSchemeHandler : IInitializable, IDisposable
{
    private readonly InputController _input;
    private readonly GameStateMachine _stateMachine;

    private InputController.ActionEvent _jump;
    private InputController.ActionEvent _pause;

    public SceneInputSchemeHandler(InputController input, GameStateMachine stateMachine)
    {
        _input = input;
        _stateMachine = stateMachine;
    }

    public void Initialize()
    {
        _jump = _input.GetAction(InputController.InputActionType.Jump);
        _pause = _input.GetAction(InputController.InputActionType.Pause);

        _stateMachine.OnStateChanged += OnStateChanged;
        ApplyForState(_stateMachine.CurrentState);
    }

    public void Dispose()
    {
        _stateMachine.OnStateChanged -= OnStateChanged;

        _jump?.Disable();
        _pause?.Disable();
    }

    private void OnStateChanged(GameState oldState, GameState newState) => ApplyForState(newState);

    private void ApplyForState(GameState state)
    {
        _jump.Disable();
        _pause.Disable();

        switch (state)
        {
            case GameplayState:
                _jump.Enable();
                _pause.Enable();
                break;

            case PausedState:
                _pause.Enable();
                break;

            case GameOverState:
                break;
        }
    }
}