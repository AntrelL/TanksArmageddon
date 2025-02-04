using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TanksArmageddon.Core.CompositionRoot
{
    public abstract class BaseCompositionRoot : BaseCompositionGroup
    {
        [SerializeField] private List<BaseCompositionGroup> _otherGroups = new();

        protected const int ExecutionOrderValue = -9000;

        private void OnValidate() => CheckExecutionOrder();

        private void Awake()
        {
            Create();
            _otherGroups.ForEach(group => group.Create());

            Construct();
            _otherGroups.ForEach(group => group.Construct());
        }

        private IEnumerator Start()
        {
            yield return LazyConstruct();

            foreach (var group in _otherGroups)
                yield return group.LazyConstruct();
        }

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
                Debug.LogWarning($"{GetType().Name} must have an execution order of " +
                    $"{ExecutionOrderValue} (const {nameof(ExecutionOrderValue)})");
            }
        }
    }
}
