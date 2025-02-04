using UnityEngine;

namespace TanksArmageddon.Core.CompositionRoot
{
    public class MonoScript : MonoBehaviour
    {
        public bool IsConstructed { get; private set; } = false;

        public bool IsLazyConstructed { get; private set; } = false;

        protected void OnConstructed()
        {
            if (IsConstructed)
            {
                Debug.LogError("The constructor can only be called once");
                return;
            }

            IsConstructed = true;
        }

        protected void OnLazyConstructed()
        {
            if (IsLazyConstructed)
            {
                Debug.LogError("The lazy constructor can only be called once");
                return;
            }

            IsLazyConstructed = true;
        }
    }
}
