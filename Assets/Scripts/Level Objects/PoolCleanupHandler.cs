using UnityEngine.SceneManagement;
using Zenject;

public class PoolCleanupHandler : IInitializable, System.IDisposable
{
    private readonly LevelObjectGenerator _generator;

    public PoolCleanupHandler(LevelObjectGenerator generator)
    {
        _generator = generator;
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
        _generator.StopSpawning();
    }
}