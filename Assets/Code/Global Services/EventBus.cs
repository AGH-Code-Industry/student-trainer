using System;
using System.Collections.Generic;

public class EventBus
{
    private readonly Dictionary<Type, List<Delegate>> _subscribers = new();

    public void Subscribe<T>(Action<T> listener)
    {
        if (!_subscribers.ContainsKey(typeof(T)))
        {
            _subscribers[typeof(T)] = new List<Delegate>();
        }
        _subscribers[typeof(T)].Add(listener);
    }

    public void Unsubscribe<T>(Action<T> listener)
    {
        if (_subscribers.ContainsKey(typeof(T)))
        {
            _subscribers[typeof(T)].Remove(listener);
        }
    }

    public void Publish<T>(T eventData)
    {
        if (_subscribers.ContainsKey(typeof(T)))
        {
            foreach (var listener in _subscribers[typeof(T)])
            {
                ((Action<T>)listener)?.Invoke(eventData);
            }
        }
    }
}
