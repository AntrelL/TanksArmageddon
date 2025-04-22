using System.Collections;
using UnityEngine;

namespace TanksArmageddon
{
    public abstract class Bootstrap : MonoBehaviour
    {
        protected const int ExecutionOrderValue = -1;

        private void OnValidate() => CheckExecutionOrder();

        private IEnumerator Start() => Initialize();

        public abstract IEnumerator Initialize();

        private void CheckExecutionOrder()
        {
            object[] attributes = GetType().GetCustomAttributes(typeof(DefaultExecutionOrder), false);

            if (attributes.Length == 0)
            {
                Debug.LogWarning($"{GetType().Name} has no established order of execution");
                return;
            }

            int executionOrder = ((DefaultExecutionOrder)attributes[0]).order;

            if (executionOrder != ExecutionOrderValue)
            {
                Debug.LogWarning(
                    $"{GetType().Name} must have an execution order of " +
                    $"{ExecutionOrderValue} (const {nameof(ExecutionOrderValue)})");
            }
        }
    }
}
