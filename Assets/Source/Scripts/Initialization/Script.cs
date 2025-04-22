using UnityEngine;

namespace TanksArmageddon
{
    public abstract class Script : MonoBehaviour
    {
        private bool _isUnprocessedOnEnable = false;

        public bool IsInitialized { get; private set; }

        private void OnEnable()
        {
            if (IsInitialized)
                OnActivated();
            else
                _isUnprocessedOnEnable = true;
        }

        private void OnDisable() => OnDeactivated();

        protected void OnInitialized()
        {
            if (IsInitialized)
            {
                Debug.LogError("The initialization method can only be called once");
                return;
            }

            IsInitialized = true;

            if (_isUnprocessedOnEnable)
            {
                OnActivated();
                _isUnprocessedOnEnable = false;
            }
        }

        protected virtual void OnActivated() { }

        protected virtual void OnDeactivated() { }
    }
}
