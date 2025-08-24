using UnityEngine.SceneManagement;
using Zenject;

public class SceneCleanupBroadcaster : IInitializable, System.IDisposable
{
    private readonly IEventBus _eventBus;

    [Inject]
    public SceneCleanupBroadcaster(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

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
        _eventBus.Publish(new GameCleanupEvent());
    }
}
