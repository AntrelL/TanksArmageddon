using System;
using UnityEngine;
using System.Collections.Generic;

namespace TanksArmageddon.CompositeRoot
{
    public class MonoScript : MonoBehaviour
    {
        private List<(Action, Action)> _links = new ();

        protected void CreateConnection<T>(ref T @event, T @delegate) where T : Delegate
        {
            T localEvent = @event;

            _links.Add((
                () => localEvent = (T)Delegate.Combine(localEvent, @delegate),
                () => localEvent = (T)Delegate.Remove(localEvent, @delegate)
            ));
        }
    }
}
