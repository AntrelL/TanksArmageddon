using UnityEngine;

public abstract class EntryPoint : MonoScript
{
    protected const int ExecutionOrderValue = -9000;

    private readonly Contract _attributeQuantityContract =
        new(" has no established order of execution", severity: ContractSeverity.Warning);

    private readonly Contract _executionOrderContract =
        new($" must have an execution order of {ExecutionOrderValue} " +
            $"(const {nameof(ExecutionOrderValue)})", severity: ContractSeverity.Warning);

    private void OnValidate() => CheckExecutionOrder();

    private void Awake() => Construct();

    protected abstract void Construct();

    private void CheckExecutionOrder()
    {
        object[] attributes = GetType().GetCustomAttributes(typeof(DefaultExecutionOrder), false);
        string typeName = GetType().Name;

        if (_attributeQuantityContract.CheckViolation(attributes.Length == 0, prefix: typeName))
            return;

        int executionOrder = ((DefaultExecutionOrder)attributes[0]).order;

        if (_executionOrderContract.CheckViolation(executionOrder != ExecutionOrderValue, prefix: typeName))
            return;
    }
}
