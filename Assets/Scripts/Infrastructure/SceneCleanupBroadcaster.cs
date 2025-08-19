using UnityEngine.SceneManagement;
using Zenject;
using UniRx;

public class SceneCleanupBroadcaster : IInitializable, System.IDisposable
{
    public void Initialize()
    {
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    public void Dispose()
    {
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }

    private void OnActiveSceneChanged(Scene oldScene, Scene newScene)
    {
        // Перед любой новой сценой шлём очистку
        GameEvents.OnGameCleanup.OnNext(Unit.Default);
    }
}
