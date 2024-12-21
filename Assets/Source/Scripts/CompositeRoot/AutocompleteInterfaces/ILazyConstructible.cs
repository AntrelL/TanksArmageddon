using System.Collections;

namespace TanksArmageddon.CompositeRoot
{
    public interface ILazyConstructible
    {
        IEnumerator LazyConstruct();
    }
}
