using MVVM;
using System;
using UniRx;
using UnityEngine.UI;

public sealed class ImageBinder : IBinder, IObserver<int>
{
    private readonly Image[] _heartImages; // ������ Image
    private readonly IReadOnlyReactiveProperty<int> _livesProperty; // �������� ������
    private IDisposable _subscription;

    // ����������� ��� � TextBinder
    public ImageBinder(Image[] heartImages, IReadOnlyReactiveProperty<int> livesProperty)
    {
        _heartImages = heartImages;
        _livesProperty = livesProperty;
    }

    public void Bind()
    {
        // ������ ����������
        UpdateHearts(_livesProperty.Value);
        // �������� �� ���������
        _subscription = _livesProperty.Subscribe(UpdateHearts);
    }

    public void Unbind() => _subscription?.Dispose();

    private void UpdateHearts(int currentHealth)
    {
        for (int i = 0; i < _heartImages.Length; i++)
        {
            bool shouldBeActive = i < currentHealth;
            _heartImages[i].gameObject.SetActive(shouldBeActive);
        }
    }

    public void OnNext(int activeHearts) => UpdateHearts(activeHearts);
    public void OnCompleted() { }
    public void OnError(Exception error) { }
}