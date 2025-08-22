using MVVM;
using UniRx;
using System;

public class AcceptDeleteProfileButtonViewModel : IDisposable
{
    [Data("AcceptDelete")]
    public readonly ReactiveCommand AcceptDelete = new();

    private readonly ProfilesSceneUIController _sceneController;
    private readonly CompositeDisposable _disposables = new();

    public AcceptDeleteProfileButtonViewModel(ProfilesSceneUIController sceneController)
    {
        _sceneController = sceneController;

        AcceptDelete
            .Subscribe(_ => _sceneController.ConfirmDeleteSelectedProfile())
            .AddTo(_disposables);
    }

    public void Dispose() => _disposables.Dispose();
}
