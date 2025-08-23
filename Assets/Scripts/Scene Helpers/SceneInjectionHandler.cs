using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using System.Collections.Generic;

public class SceneInjectionHandler : IInitializable, System.IDisposable
{
    private readonly SceneInjectionConfig _config;
    private DiContainer _sceneContainer;         // активный контейнер сцены
    private readonly DiContainer _projectContainer;
    private readonly List<GameObject> _persistentToReinject = new();

    public SceneInjectionHandler(SceneInjectionConfig config, DiContainer projectContainer)
    {
        _config = config;
        _projectContainer = projectContainer;
        _sceneContainer = projectContainer; // по умолчанию — проектный
    }

    public void Initialize()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        TrySetActiveSceneContainer();
    }

    public void Dispose()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Устанавливаем контейнер из SceneContext
    public void SetSceneContainer(DiContainer container)
    {
        _sceneContainer = container ?? _projectContainer;
        Debug.Log($"[SceneInjectionHandler] Установлен контейнер сцены: {_sceneContainer.GetHashCode()}");
    }

    public void RegisterPersistent(Object obj)
    {
        if (obj is Component comp)
            _persistentToReinject.Add(comp.gameObject);
        else if (obj is GameObject go)
            _persistentToReinject.Add(go);
        else
            Debug.LogWarning($"[SceneInjectionHandler] Не удалось зарегистрировать {obj}");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        TrySetActiveSceneContainer();

        if (_config.allowedScenes.Contains(scene.name))
        {
            Debug.Log($"[SceneInjectionHandler] Авто-реинжект персистентных объектов в сцене {scene.name}");
            ReinjectPersistent();
        }
        else
        {
            Debug.Log($"[SceneInjectionHandler] Сцена {scene.name} не разрешена для авто-реинжекта");
        }
    }

    private void TrySetActiveSceneContainer()
    {
        var sceneContext = Object.FindFirstObjectByType<SceneContext>();
        if (sceneContext != null)
        {
            _sceneContainer = sceneContext.Container;
        }
        else
        {
            _sceneContainer = _projectContainer;
            Debug.LogWarning("[SceneInjectionHandler] SceneContext не найден, используем ProjectContext");
        }
    }

    private void ReinjectPersistent()
    {
        foreach (var obj in _persistentToReinject)
        {
            if (obj != null)
                _sceneContainer.InjectGameObject(obj);
        }
    }
}