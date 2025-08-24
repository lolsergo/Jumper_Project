using UniRx;
using Zenject;

// Временный мост: код, который ещё не переведён, может остаться на GameEventsAdapter.OnGameplayStarted и т.п.
public class GameEventsAdapter : IInitializable, System.IDisposable
{
    private readonly IEventBus _bus;
    public readonly Subject<Unit> OnGameCleanup = new();
    public readonly Subject<Unit> OnGameplayStarted = new();
    public readonly Subject<Unit> OnGamePaused = new();
    public readonly Subject<Unit> OnGameResumed = new();
    public readonly Subject<Unit> OnGameOver = new();

    private readonly CompositeDisposable _disposables = new();

    [Inject]
    public GameEventsAdapter(IEventBus bus) => _bus = bus;

    public void Initialize()
    {
        _bus.Receive<GameCleanupEvent>().Subscribe(_ => OnGameCleanup.OnNext(Unit.Default)).AddTo(_disposables);
        _bus.Receive<GameplayStartedEvent>().Subscribe(_ => OnGameplayStarted.OnNext(Unit.Default)).AddTo(_disposables);
        _bus.Receive<GamePausedEvent>().Subscribe(_ => OnGamePaused.OnNext(Unit.Default)).AddTo(_disposables);
        _bus.Receive<GameResumedEvent>().Subscribe(_ => OnGameResumed.OnNext(Unit.Default)).AddTo(_disposables);
        _bus.Receive<GameOverEvent>().Subscribe(_ => OnGameOver.OnNext(Unit.Default)).AddTo(_disposables);
    }

    public void Dispose() => _disposables.Dispose();
}