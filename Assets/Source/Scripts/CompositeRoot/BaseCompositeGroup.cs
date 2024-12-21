using System.Collections;
using UnityEngine;

namespace TanksArmageddon.CompositeRoot
{
    public abstract class BaseCompositeGroup : MonoBehaviour, IConstructible, ILazyConstructible
    {
        public virtual void Construct() { }

        public virtual IEnumerator LazyConstruct() { yield return null; }
    }
}
