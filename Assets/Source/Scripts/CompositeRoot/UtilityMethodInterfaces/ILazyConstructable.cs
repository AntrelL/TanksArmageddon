using System.Collections;

namespace TanksArmageddon.CompositeRoot
{
    public interface ILazyConstructable<T>
    {
        public IEnumerator LazyConstruct(T parameters);
    }

    public interface ILazyConstructable
    {
        public IEnumerator LazyConstruct();
    }
}
