using System;

public interface IReadOnlyScale<T>
{
    T Value { get; }

    T Min { get; }

    T Max { get; }

    IProtectedEvent<Action<T>> Changed { get; }
}
