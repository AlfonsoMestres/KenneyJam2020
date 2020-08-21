using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event : IEventSubscribe
{
    event Action action;
    public void Invoke()
    {
        if (action != null) { action(); }
    }

    public void Subscribe(Action a)
    {
        action += a;
    }

    public void Unsubscribe(Action a)
    {
        action -= a;
    }

    public Delegate[] GetSubscribers()
    {
        return action == null ? new Delegate[0] : action.GetInvocationList();
    }
}

public class Event<T> : IEventSubscribe<T>
{
    event Action<T> action;
    public void Invoke(T t)
    {
        if (action != null) { action(t); }
    }

    public void Subscribe(Action<T> a)
    {
        action += a;
    }

    public void Unsubscribe(Action<T> a)
    {
        action -= a;
    }

    public Delegate[] GetSubscribers()
    {
        return action == null ? new Delegate[0] : action.GetInvocationList();
    }
}
