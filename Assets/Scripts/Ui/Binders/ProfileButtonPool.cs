using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ProfileButtonPool
{
    private readonly Transform _parent;
    private readonly ProfileButtonView _prefab;
    private readonly Stack<ProfileButtonView> _pool = new();
    private readonly DiContainer _container;

    public ProfileButtonPool(DiContainer container, ProfileButtonView prefab, Transform parent, int initialSize = 0)
    {
        _container = container;
        _prefab = prefab;
        _parent = parent;

        // Предсоздание экземпляров
        for (int i = 0; i < initialSize; i++)
        {
            var btn = CreateNew();
            Return(btn);
        }
    }

    private ProfileButtonView CreateNew()
    {
        var btn = _container.InstantiatePrefabForComponent<ProfileButtonView>(_prefab, _parent);
        btn.gameObject.SetActive(false);
        return btn;
    }

    public ProfileButtonView Get()
    {
        if (_pool.Count > 0)
            return _pool.Pop();

        return CreateNew();
    }

    public void Return(ProfileButtonView btn)
    {
        btn.gameObject.SetActive(false);
        btn.transform.SetParent(_parent, false);
        _pool.Push(btn);
    }
}