using UnityEngine;
using MVVM;

public class RuntimeViewBinder : MonoBehaviour
{
    private IBinder _binder;
    private bool _isBound;

    public void Bind(object view, object viewModel)
    {
        if (view == null)
        {
            Debug.LogError("[RuntimeViewBinder] View is null");
            return;
        }
        if (viewModel == null)
        {
            Debug.LogError("[RuntimeViewBinder] ViewModel is null");
            return;
        }

        if (_binder == null)
            _binder = BinderFactory.CreateComposite(view, viewModel);

        if (_isBound) return;

        _binder.Bind();
        _isBound = true;
    }

    public void Unbind()
    {
        if (!_isBound) return;
        _binder?.Unbind();
        _isBound = false;
    }

    private void OnEnable()
    {
        if (_binder != null && !_isBound)
            _binder.Bind();
        _isBound = _binder != null;
    }

    private void OnDestroy()
    {
        Unbind();
    }
}