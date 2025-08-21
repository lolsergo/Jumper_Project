using MVVM;
using UniRx;
using UnityEngine.UI;
using System;

public sealed class SliderBinder : IBinder, IObserver<float>
{
    private readonly Slider _slider;
    private readonly ReactiveProperty<float> _property;
    private IDisposable _subscription;
    private bool _ignoreNotify;

    public SliderBinder(Slider slider, ReactiveProperty<float> property)
    {
        _slider = slider;
        _property = property;
    }

    public void Bind()
    {
        _subscription = _property.Subscribe(this);
        _slider.onValueChanged.AddListener(OnSliderChanged);
    }

    public void Unbind()
    {
        _subscription?.Dispose();
        _slider.onValueChanged.RemoveListener(OnSliderChanged);
    }

    private void OnSliderChanged(float value)
    {
        if (_ignoreNotify) return;
        _property.Value = value;
    }

    public void OnNext(float value)
    {
        _ignoreNotify = true;
        _slider.value = value;
        _ignoreNotify = false;
    }

    public void OnCompleted() { }
    public void OnError(Exception error) { }
}