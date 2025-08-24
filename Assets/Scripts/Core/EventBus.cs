using System;
using System.Collections.Concurrent;
using UniRx;

public interface IEventBus : IDisposable
{
    void Publish<TEvent>(TEvent evt);
    IObservable<TEvent> Receive<TEvent>();
}

public class EventBus : IEventBus
{
    private readonly ConcurrentDictionary<Type, object> _subjects = new();

    public void Publish<TEvent>(TEvent evt)
    {
        if (_subjects.TryGetValue(typeof(TEvent), out var obj))
        {
            var subject = (ISubject<TEvent>)obj;
            subject.OnNext(evt);
        }
    }

    public IObservable<TEvent> Receive<TEvent>()
    {
        var subject = (ISubject<TEvent>)_subjects.GetOrAdd(typeof(TEvent), _ => new Subject<TEvent>());
        return subject;
    }

    public void Dispose()
    {
        foreach (var kv in _subjects)
            (kv.Value as IDisposable)?.Dispose();
        _subjects.Clear();
    }
}
