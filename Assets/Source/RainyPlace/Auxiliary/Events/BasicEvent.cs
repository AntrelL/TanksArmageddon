using System;

namespace RainyPlace
{
    public abstract class BasicEvent<T> : IProtectedEvent<T> where T : Delegate
    {
        protected T EventHandlers;

        public void Subscribe(T handler)
        {
            EventHandlers = (T)Delegate.Combine(EventHandlers, handler);
        }

        public virtual void Unsubscribe(T handler)
        {
            EventHandlers = (T)Delegate.Remove(EventHandlers, handler);
        }
    }
}
