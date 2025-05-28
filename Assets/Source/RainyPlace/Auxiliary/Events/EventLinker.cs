using System;
using System.Collections.Generic;

namespace RainyPlace
{
    public class EventLinker
    {
        private Dictionary<
            (int EventHash, Delegate Handler), 
            (Action Subscription, Action Unsubscription)> _links = new();

        public void AddLink<T>(IProtectedEvent<T> @event, T handler) where T : Delegate
        {
            _links.Add(GetLinkKey(@event, handler), (
                () => @event.Subscribe(handler),
                () => @event.Unsubscribe(handler)));
        }

        public void RemoveLink<T>(IProtectedEvent<T> @event, T handler) where T : Delegate
        {
            _links.Remove(GetLinkKey(@event, handler));
        }

        public void Subscribe()
        {
            foreach (var link in _links.Values)
                link.Subscription.Invoke();    
        }

        public void Unsubscribe()
        {
            foreach (var link in _links.Values)
                link.Unsubscription.Invoke();
        }

        private (int, Delegate) GetLinkKey<T>(IProtectedEvent<T> @event, T handler) where T : Delegate
        {
            return (@event.GetHashCode(), handler);
        }
    }
}
