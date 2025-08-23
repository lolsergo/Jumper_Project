using System;
using UniRx;
using UnityEngine;
using Zenject;
using MVVM;

public class ProfilesContainerBinder : IBinder
{
    private readonly Transform _containerTransform;
    private readonly ReactiveCollection<ProfileButtonViewModel> _profiles;
    private readonly DiContainer _diContainer;
    private ProfileButtonPool _pool;

    private readonly CompositeDisposable _disposables = new();

    public ProfilesContainerBinder(
        Transform container,
        ReactiveCollection<ProfileButtonViewModel> profiles,
        DiContainer diContainer)
    {
        _containerTransform = container;
        _profiles = profiles;
        _diContainer = diContainer;
    }

    public void Bind()
    {
        var parentView = _containerTransform != null
            ? _containerTransform.GetComponentInParent<ProfilesListView>(true)
            : null;

        if (parentView == null)
            parentView = UnityEngine.Object.FindAnyObjectByType<ProfilesListView>();

        if (parentView == null || parentView.ButtonPrefab == null)
            throw new InvalidOperationException("[ProfilesContainerBinder] ButtonPrefab");

        _pool = new ProfileButtonPool(_diContainer, parentView.ButtonPrefab, _containerTransform, initialSize: _profiles.Count);

        _profiles.ObserveAdd().Subscribe(x => AddButton(x.Value)).AddTo(_disposables);
        _profiles.ObserveRemove().Subscribe(_ => Refresh()).AddTo(_disposables);
        _profiles.ObserveReset().Subscribe(_ => Refresh()).AddTo(_disposables);

        Refresh();
    }

    public void Unbind()
    {
        _disposables.Clear();
    }

    private void Refresh()
    {
        foreach (Transform child in _containerTransform)
        {
            var view = child.GetComponent<ProfileButtonView>();
            if (view != null)
                _pool.Return(view);
        }

        foreach (var vm in _profiles)
            AddButton(vm);
    }

    private void AddButton(ProfileButtonViewModel vm)
    {
        var btnView = _pool.Get();
        btnView.gameObject.SetActive(true);

        var label = btnView.Label != null
            ? btnView.Label
            : btnView.GetComponentInChildren<TMPro.TMP_Text>(true);

        var button = btnView.Button != null
            ? btnView.Button
            : btnView.GetComponentInChildren<UnityEngine.UI.Button>(true);

        var binder = new ProfileButtonBinder(label, button, vm);
        binder.Bind();
    }
}