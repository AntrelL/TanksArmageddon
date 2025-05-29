using System;

namespace RainyPlace.DI
{
    public class Script
    {
        private readonly Contract _enableContract = new("The object is already enabled");
        private readonly Contract _disableContract = new("The object is already disabled");

        private EventLinker _eventLinker = new();

        public bool IsEnabled { get; set; }

        public void Enable()
        {
            if (_enableContract.CheckViolation(IsEnabled)) 
                return;

            IsEnabled = true;

            _eventLinker.Subscribe();
            OnEnable();
        }

        public void Disable()
        {
            if (_disableContract.CheckViolation(IsEnabled == false))
                return;

            IsEnabled = false;

            _eventLinker.Unsubscribe();
            OnDisable();
        }

        protected void Link<T>(IProtectedEvent<T> @event, T handler) where T : Delegate
        {
            _eventLinker.AddLink(@event, handler);
        }

        protected void Unlink<T>(IProtectedEvent<T> @event, T handler) where T : Delegate
        {
            _eventLinker.RemoveLink(@event, handler);
        }

        public virtual void Update(float deltaTime) { }

        protected virtual void OnEnable() { }

        protected virtual void OnDisable() { }
    }
}
