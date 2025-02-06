using System.Collections;
using TanksArmageddon.Core.CompositionRoot;
using UnityEngine;

namespace TanksArmageddon
{
    [DefaultExecutionOrder(ExecutionOrderValue)]
    public class MainCompositionRoot : BaseCompositionRoot
    {
        [SerializeField] private UICompositionGroup _UICompositionGroup;

        public override void CreateEnvironment()
        {
        }

        public override void Construct()
        {
            _UICompositionGroup.Construct();
        }

        public override IEnumerator LazyConstruct()
        {
            yield return null;
        }
    }
}
