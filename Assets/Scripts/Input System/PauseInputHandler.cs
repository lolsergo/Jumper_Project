using System;
using Zenject;

public class PauseInputHandler : IInitializable, IDisposable
{
    private readonly InputController.ActionEvent _pauseAction;
    private readonly GameManager _gameManager;

    public PauseInputHandler(InputController inputController, GameManager gameManager)
    {
        _pauseAction = inputController.GetAction(InputController.InputActionType.Pause);
        _gameManager = gameManager;
    }

    public void Initialize()
    {
        _pauseAction.OnPressed += HandlePausePressed;
    }

    public void Dispose()
    {
        _pauseAction.OnPressed -= HandlePausePressed;
    }

    private void HandlePausePressed() => _gameManager.HandlePauseInput();
}
