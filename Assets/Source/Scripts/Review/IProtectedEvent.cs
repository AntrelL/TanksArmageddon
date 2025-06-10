using System;

public interface IProtectedEvent<T> where T : Delegate
{
    void Subscribe(T handler);

    void Unsubscribe(T handler);
}
