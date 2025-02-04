using UnityEngine;

namespace TanksArmageddon.Core.CompositionRoot
{
    public class MonoScript : MonoBehaviour
    {
        private bool _isConstructed = false;
        private bool _isLazyConstructed = false;

        protected void OnConstructed()
        {
            if (_isConstructed)
            {
                Debug.LogError("The constructor can only be called once");
                return;
            }

            _isConstructed = true;
        }

        protected void OnLazyConstructed()
        {
            if (_isLazyConstructed)
            {
                Debug.LogError("The lazy constructor can only be called once");
                return;
            }

            _isLazyConstructed = true;
        }
    }
}
