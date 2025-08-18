using MVVM;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public sealed class BuyHealthButtonBinder : IBinder
{
    private readonly Button _button;
    private readonly Image _buttonImage;
    private readonly Action _modelAction;
    private readonly ColorReactiveProperty _buttonColor;

    public BuyHealthButtonBinder(
        Button button,
        Action model,
        ColorReactiveProperty buttonColor)
    {
        _button = button;
        _buttonImage = button.GetComponent<Image>();
        _modelAction = model;
        _buttonColor = buttonColor;
    }

    void IBinder.Bind()
    {
        _button.onClick.AddListener(() => _modelAction?.Invoke());

        // �������������� ��������� �����
        _buttonImage.color = _buttonColor.Value;
        _button.interactable = (_buttonColor.Value != Color.gray);

        // ����� �������� ����������, ���� ����� ������������ ����������
        // (�� � ������� ������ ���� �������� ������ ���� ���)
    }

    public void Unbind()
    {
        _button.onClick.RemoveAllListeners();
    }
}
