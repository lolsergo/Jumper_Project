using MVVM;
using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class ProfilesContainerBinder : IBinder
{
    private readonly Transform _container;
    private readonly ReactiveCollection<ProfileButtonViewModel> _profiles;
    private ProfileButtonPool _pool;

    private readonly CompositeDisposable _disposables = new();

    public ProfilesContainerBinder(
        Transform container,
        ReactiveCollection<ProfileButtonViewModel> profiles)
    {
        _container = container;
        _profiles = profiles;
    }

    public void Bind()
    {
        var parentView = _container != null
            ? _container.GetComponentInParent<ProfilesListView>(true)
            : null;

        if (parentView == null)
            parentView = UnityEngine.Object.FindAnyObjectByType<ProfilesListView>();

        if (parentView == null || parentView.ButtonPrefab == null)
            throw new InvalidOperationException("[ProfilesContainerBinder] Не найден ProfilesListView или ButtonPrefab");

        _pool = new ProfileButtonPool(parentView.ButtonPrefab, _container, initialSize: _profiles.Count);

        _profiles.ObserveAdd().Subscribe(x => AddButton(x.Value)).AddTo(_disposables);
        _profiles.ObserveRemove().Subscribe(_ => Refresh()).AddTo(_disposables);
        _profiles.ObserveReset().Subscribe(_ => Refresh()).AddTo(_disposables);

        Refresh();
    }

    public void Unbind()
    {
        _disposables.Clear();
        // По желанию можно очищать пул
    }

    private void Refresh()
    {
        // Возвращаем все кнопки в пул
        foreach (Transform child in _container)
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

        var label = btnView.GetComponentInChildren<TMP_Text>(true);
        var button = btnView.GetComponentInChildren<Button>(true);

        var binder = new ProfileButtonBinder(label, button, vm);
        binder.Bind();
    }
}