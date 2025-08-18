using UnityEngine;
using UnityEngine.UI;
using UniRx;
using MVVM;

public sealed class ImageColorBinder : IBinder
{
    private readonly Image _image;
    private readonly IReadOnlyReactiveProperty<Color> _colorProperty;
    private readonly CompositeDisposable _disposables = new();

    public ImageColorBinder(Image image, IReadOnlyReactiveProperty<Color> colorProperty)
    {
        _image = image;
        _colorProperty = colorProperty;
    }

    public void Bind()
    {
        _colorProperty.Subscribe(color => {
            _image.color = color;
            Debug.Log($"Color updated to {color}", _image);
        }).AddTo(_disposables);
    }

    public void Unbind() => _disposables.Dispose();
}
