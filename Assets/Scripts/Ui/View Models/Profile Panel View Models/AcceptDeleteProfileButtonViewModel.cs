using MVVM;
using UniRx;
using System;

public class AcceptDeleteProfileButtonViewModel : IDisposable
{
    [Data("AcceptDelete")]
    public readonly ReactiveCommand ConfirmDelete = new();

    private readonly ProfilesSceneController _sceneController;
    private readonly CompositeDisposable _disposables = new();

    public AcceptDeleteProfileButtonViewModel(ProfilesSceneController sceneController)
    {
        _sceneController = sceneController;

        ConfirmDelete
            .Subscribe(_ => _sceneController.ConfirmDeleteSelectedProfile())
            .AddTo(_disposables);
    }

    public void Dispose() => _disposables.Dispose();
}
