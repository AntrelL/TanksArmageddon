using System.Collections;
using TanksArmageddon.Core.CompositionRoot;
using TanksArmageddon.Core.PrefabControl;

namespace TanksArmageddon
{
    public class UICompositionGroup : BaseCompositionGroup
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
