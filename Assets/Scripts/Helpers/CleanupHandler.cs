using UniRx;
using UnityEngine;
using Zenject;

public class CleanupHandler : MonoBehaviour
{
    private CompositeDisposable _disposables = new();
    private IEventBus _eventBus;

    [Inject]
    private void Construct(IEventBus eventBus)
    {
        _eventBus = eventBus;

        // ѕодписка перенесена сюда Ч гарантированно после инъекции
        _eventBus
            .Receive<GameCleanupEvent>()
            .Subscribe(_ => Cleanup())
            .AddTo(_disposables);
    }

    // ”далЄн OnEnable Ч он вызывалс€ слишком рано

    private void Cleanup()
    {
        _disposables.Clear();
        Destroy(gameObject);
    }
}
