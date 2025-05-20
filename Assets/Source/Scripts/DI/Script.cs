using UnityEngine;

namespace TanksArmageddon.DI
{
    public abstract class Script : MonoBehaviour
    {
        private bool _isUnprocessedOnEnable = false;

        public bool IsConstructed { get; private set; }

        private void OnEnable()
        {
            if (IsConstructed)
                OnActivate();
            else
                _isUnprocessedOnEnable = true;
        }

        private void OnDisable() => OnDeactivate();

        protected void OnConstructed()
        {
            if (IsConstructed)
            {
                Debug.Log("The constructor can only be called once");
                return;
            }

            IsConstructed = true;

            if (_isUnprocessedOnEnable)
            {
                OnActivate();
                _isUnprocessedOnEnable = false;
            }
        }

        protected virtual void OnActivate() { }

        protected virtual void OnDeactivate() { }
    }
}
