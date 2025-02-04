using System.Collections;
using TanksArmageddon.Core.CompositionRoot;
using UnityEngine;

namespace TanksArmageddon
{
    [DefaultExecutionOrder(ExecutionOrderValue)]
    public class TestCompositionRoot : BaseCompositionRoot
    {
        [SerializeField] private FirstCompositionGroup _firstCompositionGroup;

        public override void CreateEnvironment()
        {
            _firstCompositionGroup.CreateEnvironment();
        }

        public override void Construct()
        {
            _firstCompositionGroup.Construct();
        }

        public override IEnumerator LazyConstruct()
        {
            yield return _firstCompositionGroup.LazyConstruct();
        }
    }
}
