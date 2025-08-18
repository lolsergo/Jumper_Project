using MVVM;
using UniRx;
using UnityEngine.UI;

public sealed class AudioBinder : IBinder
{
    private readonly Button _button;
    private readonly string _soundID;
    private readonly ReactiveCommand<string> _command;

    public AudioBinder(Button button, string soundId, ReactiveCommand<string> command)
    {
        _button = button;
        _soundID = soundId;
        _command = command;
    }

    public void Bind()
    {
        _button.onClick.AsObservable()
            .Subscribe(_ => _command.Execute(_soundID))
            .AddTo(_button);
    }

    public void Unbind()
    {
        _button.onClick.RemoveAllListeners();
    }
}