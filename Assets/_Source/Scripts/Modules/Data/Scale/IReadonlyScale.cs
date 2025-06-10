using System;

namespace RainyPlace.Internal
{
    public interface IReadonlyScale<T>
    {
        event Action<T> ValueChanged;
        
        event Action<(T Value, T Min, T Max)> Changed;
        
        T Value { get; }
        
        T Min { get; }
        
        T Max { get; }
    }
}
