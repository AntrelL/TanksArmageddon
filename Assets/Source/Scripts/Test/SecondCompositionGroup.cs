using System.Collections;
using TanksArmageddon.Core.CompositionRoot;

namespace TanksArmageddon
{
    public class SecondCompositionGroup : BaseCompositionGroup
    {
        public override void CreateEnvironment()
        {
        }

        public override void Construct()
        {
        }

        public override IEnumerator LazyConstruct()
        {
            yield return null;
        }
    }
}
