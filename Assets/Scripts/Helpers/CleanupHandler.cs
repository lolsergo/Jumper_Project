using UniRx;
using UnityEngine;

public class CleanupHandler : MonoBehaviour
{
    private CompositeDisposable _disposables = new();

    private void OnEnable()
    {
        GameEvents.OnGameCleanup.Subscribe(_ => Cleanup()).AddTo(_disposables);
    }

    private void Cleanup()
    {
        _disposables.Clear();
        Destroy(gameObject); // или отключение, если нужно сохранить
    }
}
