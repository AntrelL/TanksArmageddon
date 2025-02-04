using System.Collections;
using UnityEngine;

namespace TanksArmageddon.Core.CompositionRoot
{
    public abstract class BaseCompositionGroup : MonoBehaviour
    {
        public virtual void Create() { }

        public virtual void Construct() { }

        public virtual IEnumerator LazyConstruct()
        {
            yield return null;
        }
    }
}
