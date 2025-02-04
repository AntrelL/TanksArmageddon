using System.Collections;

namespace TanksArmageddon.Core.CompositionRoot
{
    public interface ILazyConstructible
    {
        IEnumerator LazyConstruct();
    }
}
