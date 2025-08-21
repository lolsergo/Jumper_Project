using MVVM;
using System;
using UniRx;
using UnityEngine;

public sealed class ProfilesCollectionBinder : IBinder
{
    private readonly Transform _container;
    private readonly ReactiveCollection<ProfileButtonViewModel> _models;
    private readonly ProfilesListView _listView;
    private readonly CompositeDisposable _disposables = new();

    public ProfilesCollectionBinder(Transform container, ReactiveCollection<ProfileButtonViewModel> models)
    {
        _container = container;
        _models = models;
        _listView = container.GetComponentInParent<ProfilesListView>();
    }

    public void Bind()
    {
        foreach (var model in _models)
            AddButton(model);

        _models.ObserveAdd()
               .Subscribe(x => AddButton(x.Value))
               .AddTo(_disposables);

        _models.ObserveRemove()
               .Subscribe(_ => ClearAll())
               .AddTo(_disposables);

        _models.ObserveReset()
               .Subscribe(_ => ClearAll())
               .AddTo(_disposables);
    }

    public void Unbind()
    {
        _disposables.Clear();
        ClearAll();
    }

    private void AddButton(ProfileButtonViewModel model)
    {
        var view = UnityEngine.Object.Instantiate(_listView.ButtonPrefab, _container);
        var composite = BinderFactory.CreateComposite(view, model);
        composite.Bind();
    }

    private void ClearAll()
    {
        foreach (Transform child in _container)
            UnityEngine.Object.Destroy(child.gameObject);
    }
}
