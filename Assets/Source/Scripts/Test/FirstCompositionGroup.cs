using System.Collections;
using TanksArmageddon.Core.CompositionRoot;
using UnityEngine;

namespace TanksArmageddon
{
    public class FirstCompositionGroup : BaseCompositionGroup
    {
        [SerializeField] private TestObject1 _testObject1;

        public override void Create()
        {
        }

        public override void Construct()
        {
            _testObject1.Construct("text");
        }

        public override IEnumerator LazyConstruct()
        {
            yield return null;
        }
    }
}
