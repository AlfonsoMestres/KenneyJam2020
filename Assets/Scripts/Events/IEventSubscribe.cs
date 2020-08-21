using System;

public interface IEventSubscribe
{
    void Subscribe(Action a);
    void Unsubscribe(Action a);
    Delegate[] GetSubscribers();
}

public interface IEventSubscribe<T>
{
    void Subscribe(Action<T> a);
    void Unsubscribe(Action<T> a);
    Delegate[] GetSubscribers();
}
