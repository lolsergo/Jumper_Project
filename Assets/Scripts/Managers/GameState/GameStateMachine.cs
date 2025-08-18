using System;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine
{
    private GameState _currentState;

    public GameState CurrentState => _currentState;

    public void ChangeState(GameState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    public void Update()
    {
        _currentState?.Update();
    }
}
