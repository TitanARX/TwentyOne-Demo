using System.Collections.Generic;
using System;

public interface IEvent { }

public struct MyEvent : IEvent
{
    public string message;
}

public static class EventBus
{
    private static Dictionary<Type, Action<IEvent>> _subscribers = new Dictionary<Type, Action<IEvent>>();

    public static void Subscribe<T>(Action<T> listener) where T : IEvent
    {
        Type type = typeof(T);
        if (!_subscribers.ContainsKey(type))
            _subscribers[type] = (x) => listener((T)x);
        else
            _subscribers[type] += (x) => listener((T)x);
    }

    public static void Unsubscribe<T>(Action<T> listener) where T : IEvent
    {
        Type type = typeof(T);
        if (_subscribers.ContainsKey(type))
            _subscribers[type] -= (x) => listener((T)x);
    }

    public static void Publish<T>(T publishedEvent) where T : IEvent
    {
        Type type = typeof(T);
        if (_subscribers.ContainsKey(type))
            _subscribers[type](publishedEvent);
    }
}
