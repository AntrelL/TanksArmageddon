using System;

namespace RainyPlace
{
    public class EventWrapper<T> : IProtectedEvent<T> where T : Delegate
    {
        private Action<T> _subscription;
        private Action<T> _unsubscription;

        public EventWrapper(Action<T> subscription, Action<T> unsubscription) 
        {
            _subscription = subscription;
            _unsubscription = unsubscription;
        }

        public void Subscribe(T handler) => _subscription.Invoke(handler);

        public void Unsubscribe(T handler) => _unsubscription.Invoke(handler);
    }
}
