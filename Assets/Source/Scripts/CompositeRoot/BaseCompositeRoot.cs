using System.Collections;

namespace TanksArmageddon.CompositeRoot
{
    public abstract class BaseCompositeRoot : BaseCompositeGroup
    {
        private void Awake() => Construct();

        private IEnumerator Start() => LazyConstruct();
    }
}
