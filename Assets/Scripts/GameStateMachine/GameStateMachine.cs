using System;

public class GameStateMachine
{
    private GameState _currentState;

    public GameState CurrentState => _currentState;

    public event Action<GameState, GameState> OnStateChanged;

    public void ChangeState(GameState newState)
    {
        if (newState == null || newState == _currentState) return;

        var oldState = _currentState;

        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();

        OnStateChanged?.Invoke(oldState, _currentState);
    }

    public void StartWith(GameState initialState)
    {
        if (_currentState != null) return;
        _currentState = initialState;
        _currentState.Enter();
        OnStateChanged?.Invoke(null, _currentState);
    }

    public void Update()
    {
        _currentState?.Update();
    }

    public bool IsIn<TState>() where TState : GameState => _currentState is TState;
}
