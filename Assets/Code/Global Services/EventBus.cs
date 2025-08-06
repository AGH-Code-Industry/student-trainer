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
            // Added .ToArray() to fix error "collection was modified, enumeration operation may not execute"
            // That happens when you unsubscribe in a function that is called by the event bus (e.g. ReachAreaStep class)
            foreach (var listener in _subscribers[typeof(T)].ToArray())
            {
                ((Action<T>)listener)?.Invoke(eventData);
            }
        }
    }
}
