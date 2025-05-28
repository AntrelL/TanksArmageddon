using System;

namespace RainyPlace
{
    public class Event : BasicEvent<Action>
    {
        public void Invoke() => 
            EventHandlers?.Invoke();
    }

    public class Event<T> : BasicEvent<Action<T>>
    {
        public void Invoke(T arg) => 
            EventHandlers?.Invoke(arg);
    }

    public class Event<T1, T2> : BasicEvent<Action<T1, T2>>
    {
        public void Invoke(T1 arg1, T2 arg2) => 
            EventHandlers?.Invoke(arg1, arg2);
    }

    public class Event<T1, T2, T3> : BasicEvent<Action<T1, T2, T3>>
    {
        public void Invoke(T1 arg1, T2 arg2, T3 arg3) => 
            EventHandlers?.Invoke(arg1, arg2, arg3);
    }

    public class Event<T1, T2, T3, T4> : BasicEvent<Action<T1, T2, T3, T4>>
    {
        public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4) => 
            EventHandlers?.Invoke(arg1, arg2, arg3, arg4);
    }
}
