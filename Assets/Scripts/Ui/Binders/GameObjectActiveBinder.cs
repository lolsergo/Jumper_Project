using MVVM;
using System;
using UniRx;
using UnityEngine;

public sealed class GameObjectActiveBinder : IBinder
{
    private readonly GameObject _target;
    private readonly IObservable<bool> _source;
    private bool _isBound;

    // �������� ����������� ��� ������ ���� ReactiveProperty<bool>
    public GameObjectActiveBinder(GameObject target, ReactiveProperty<bool> source)
    {
        _target = target;
        _source = source;
    }

    // ������ ����������� ��� ����� ����� ���� IObservable<bool>
    public GameObjectActiveBinder(GameObject target, IObservable<bool> source)
    {
        _target = target;
        _source = source;
    }

    // ����������� �� ������, ���� ������� �������� � �������� �������
    public GameObjectActiveBinder(ReactiveProperty<bool> source, GameObject target)
    {
        _target = target;
        _source = source;
    }

    public void Bind()
    {
        if (_isBound) return;

        _source
            .StartWith(_target.activeSelf) // ��������� �������� = ������� �������� ���������
            .Subscribe(v =>
            {
                _target.SetActive(v);
            })
            .AddTo(_target);

        _isBound = true;
    }

    public void Unbind()
    {
        _isBound = false;
    }
}
