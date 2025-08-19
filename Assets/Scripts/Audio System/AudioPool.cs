using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class AudioPool : IMemoryPool<AudioSource>
{
    private readonly DiContainer _container;
    private readonly GameObject _prefab;
    private readonly Transform _parent;
    private readonly Stack<AudioSource> _inactiveItems = new();
    private readonly List<AudioSource> _activeItems = new();

    public int NumTotal => _activeItems.Count + _inactiveItems.Count;
    public int NumActive => _activeItems.Count;
    public int NumInactive => _inactiveItems.Count;
    public Type ItemType => typeof(AudioSource);

    public AudioPool(DiContainer container, GameObject prefab, Transform parent)
    {
        _container = container;
        _prefab = prefab;
        _parent = parent;
    }

    public AudioSource Spawn()
    {
        AudioSource item;

        if (_inactiveItems.Count > 0)
        {
            item = _inactiveItems.Pop();
            item.gameObject.SetActive(true);
        }
        else
        {
            item = _prefab != null
                ? _container.InstantiatePrefab(_prefab, _parent).GetComponent<AudioSource>()
                : _container.InstantiateComponentOnNewGameObject<AudioSource>();
        }

        item.gameObject.name = $"AudioSource_{NumTotal}";
        _activeItems.Add(item);
        return item;
    }

    public void Despawn(AudioSource item)
    {
        if (item == null) return;

        item.Stop();
        item.gameObject.SetActive(false);
        _activeItems.Remove(item);
        _inactiveItems.Push(item);
    }

    public void Resize(int desiredPoolSize)
    {
        while (_inactiveItems.Count > desiredPoolSize)
        {
            var item = _inactiveItems.Pop();
            if (item != null) GameObject.Destroy(item.gameObject);
        }
    }

    public void Clear()
    {
        foreach (var item in _inactiveItems)
        {
            if (item != null) GameObject.Destroy(item.gameObject);
        }
        _inactiveItems.Clear();
    }

    public void ExpandBy(int numToAdd)
    {
        for (int i = 0; i < numToAdd; i++)
        {
            var item = _container.InstantiateComponentOnNewGameObject<AudioSource>();
            item.gameObject.SetActive(false);
            _inactiveItems.Push(item);
        }
    }

    public void ShrinkBy(int numToRemove)
    {
        numToRemove = Math.Min(numToRemove, _inactiveItems.Count);
        for (int i = 0; i < numToRemove; i++)
        {
            var item = _inactiveItems.Pop();
            if (item != null) GameObject.Destroy(item.gameObject);
        }
    }

    void IMemoryPool.Despawn(object item) => Despawn((AudioSource)item);
}