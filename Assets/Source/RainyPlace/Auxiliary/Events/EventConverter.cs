using System;
using UnityEngine.Events;

namespace RainyPlace
{
    public static class EventConverter
    {
        public static IProtectedEvent<Action> GetProtectedEvent(
            UnityEvent unityEvent)
        {
            return new EventWrapper<Action>(
                action => unityEvent.AddListener(new(action)),
                action => unityEvent.RemoveListener(new(action)));
        }

        public static IProtectedEvent<Action<T>> GetProtectedEvent<T>(
            UnityEvent<T> unityEvent)
        {
            return new EventWrapper<Action<T>>(
                action => unityEvent.AddListener(new(action)),
                action => unityEvent.RemoveListener(new(action)));
        }

        public static IProtectedEvent<Action<T1, T2>> GetProtectedEvent<T1, T2>(
            UnityEvent<T1, T2> unityEvent)
        {
            return new EventWrapper<Action<T1, T2>>(
                action => unityEvent.AddListener(new(action)),
                action => unityEvent.RemoveListener(new(action)));
        }

        public static IProtectedEvent<Action<T1, T2, T3>> GetProtectedEvent<T1, T2, T3>(
            UnityEvent<T1, T2, T3> unityEvent)
        {
            return new EventWrapper<Action<T1, T2, T3>>(
                action => unityEvent.AddListener(new(action)),
                action => unityEvent.RemoveListener(new(action)));
        }

        public static IProtectedEvent<Action<T1, T2, T3, T4>> GetProtectedEvent<T1, T2, T3, T4>(
            UnityEvent<T1, T2, T3, T4> unityEvent)
        {
            return new EventWrapper<Action<T1, T2, T3, T4>>(
                action => unityEvent.AddListener(new(action)),
                action => unityEvent.RemoveListener(new(action)));
        }
    }
}
