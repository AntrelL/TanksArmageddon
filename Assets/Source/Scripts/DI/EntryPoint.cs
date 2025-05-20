using UnityEngine;

namespace TanksArmageddon.DI
{
    public abstract class EntryPoint : MonoBehaviour
    {
        protected const int ExecutionOrderValue = -9000;

        private void OnValidate() => CheckExecutionOrder();

        private void Awake() => Construct();

        protected abstract void Construct();

        private void CheckExecutionOrder()
        {
            object[] attributes = GetType().GetCustomAttributes(typeof(DefaultExecutionOrder), false);

            if (attributes.Length == 0)
            {
                Debug.Log($"{GetType().Name} has no established order of execution");
                return;
            }

            int executionOrder = ((DefaultExecutionOrder)attributes[0]).order;

            if (executionOrder != ExecutionOrderValue)
            {
                Debug.Log(
                    $"{GetType().Name} must have an execution order of " +
                    $"{ExecutionOrderValue} (const {nameof(ExecutionOrderValue)})");
            }
        }
    }
}
