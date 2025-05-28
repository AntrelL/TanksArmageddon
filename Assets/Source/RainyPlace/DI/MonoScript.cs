using System;
using UnityEngine;

namespace RainyPlace.DI
{
    public class MonoScript : MonoBehaviour
    {
        private readonly Contract ConstructorCallContract = 
            new("The constructor can only be called once");

        private bool _isUnprocessedOnEnable = false;
        private EventLinker _eventLinker = new();

        public bool IsConstructed { get; private set; }

        private void OnEnable()
        {
            if (IsConstructed)
                Activate();
            else
                _isUnprocessedOnEnable = true;
        }

        private void OnDisable() => Deactivate();

        protected void OnConstructed()
        {
            if (ConstructorCallContract.CheckViolation(IsConstructed))
                return;

            IsConstructed = true;

            if (_isUnprocessedOnEnable)
            {
                Activate();
                _isUnprocessedOnEnable = false;
            }
        }

        protected void Link<T>(IProtectedEvent<T> @event, T handler) where T : Delegate
        {
            _eventLinker.AddLink(@event, handler);
        }

        protected void Unlink<T>(IProtectedEvent<T> @event, T handler) where T : Delegate
        {
            _eventLinker.RemoveLink(@event, handler);
        }

        protected virtual void OnActivate() { }

        protected virtual void OnDeactivate() { }

        private void Activate()
        {
            _eventLinker.Subscribe();
            OnActivate();
        }

        private void Deactivate()
        {
            _eventLinker.Unsubscribe();
            OnDeactivate();
        }
    }
}
