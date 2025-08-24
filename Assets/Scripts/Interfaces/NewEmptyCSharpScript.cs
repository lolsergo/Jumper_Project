using System;
public interface IEventBus : IDisposable
{
    void Publish<TEvent>(TEvent evt);
    IObservable<TEvent> Receive<TEvent>();
}