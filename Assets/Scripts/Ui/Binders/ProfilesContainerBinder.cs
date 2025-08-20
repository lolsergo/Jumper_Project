// Binder контейнера кнопок профилей
using MVVM;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ProfilesContainerBinder : IBinder
{
    private readonly Transform _container;
    private readonly ReactiveCollection<ProfileButtonViewModel> _profiles;
    private ProfileButtonView _prefab;

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
        // Находим prefab
        if (_prefab == null)
        {
            var parentView = _container != null
                ? _container.GetComponentInParent<ProfilesListView>(true)
                : null;

            if (parentView == null)
                parentView = UnityEngine.Object.FindAnyObjectByType<ProfilesListView>();

            if (parentView == null || parentView.ButtonPrefab == null)
                throw new InvalidOperationException("[ProfilesContainerBinder] Не найден ProfilesListView или ButtonPrefab");

            _prefab = parentView.ButtonPrefab;
        }

        // Подписки на изменения
        _profiles.ObserveAdd().Subscribe(x => AddButton(x.Value)).AddTo(_disposables);
        _profiles.ObserveRemove().Subscribe(_ => Refresh()).AddTo(_disposables);
        _profiles.ObserveReset().Subscribe(_ => Refresh()).AddTo(_disposables);

        Refresh();
    }

    public void Unbind() => _disposables.Clear();

    private void Refresh()
    {
        foreach (Transform child in _container)
            UnityEngine.Object.Destroy(child.gameObject);

        foreach (var vm in _profiles)
            AddButton(vm);
    }

    private void AddButton(ProfileButtonViewModel vm)
    {
        var btnView = UnityEngine.Object.Instantiate(_prefab, _container);

        var label = btnView.GetComponentInChildren<TMP_Text>(true);
        var button = btnView.GetComponentInChildren<Button>(true);

        var binder = new ProfileButtonBinder(label, button, vm);
        binder.Bind();
    }
}