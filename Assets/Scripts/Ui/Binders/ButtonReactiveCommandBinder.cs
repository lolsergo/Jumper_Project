using MVVM;
using UnityEngine.UI;
using UniRx;

public sealed class ButtonReactiveCommandBinder : IBinder
{
    private readonly Button _button;
    private readonly ReactiveCommand _command;
    private bool _isBound;

    public ButtonReactiveCommandBinder(Button button, ReactiveCommand command)
    {
        _button = button;
        _command = command;
    }

    void IBinder.Bind()
    {
        if (_isBound) return;

        // �������� �� CanExecute (IObservable<bool>)
        _command.CanExecute
            .Subscribe(canExecute => _button.interactable = canExecute)
            .AddTo(_button); // Dispose ������ � �������

        _button.onClick.AddListener(OnClick);
        _isBound = true;
    }

    private void OnClick()
    {
        // �������� ���������� �� ����������
        if (_command.CanExecute.Value) // � ReactiveCommand<T> ��� ����� ��� ReactiveProperty<bool>
            _command.Execute();
    }

    public void Unbind()
    {
        if (!_isBound) return;

        _button.onClick.RemoveListener(OnClick);
        _isBound = false;
    }
}