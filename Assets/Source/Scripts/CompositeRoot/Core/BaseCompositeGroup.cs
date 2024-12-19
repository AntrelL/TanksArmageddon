using System.Collections;
using System.Collections.Generic;
using TanksArmageddon.SceneControl.Level;

namespace TanksArmageddon.CompositeRoot.Core
{
    public abstract class BaseCompositeGroup : 
        MonoScript, IConstructable, ILazyConstructable, ISwitchable, ILinkable
    {
        public List<IUpdatable> Updatables { get; private set; }

        public List<IFixedUpdatable> FixedUpdatables { get; private set; }

        public List<IFixedUpdatable> Linkables { get; private set; }

        public virtual void Construct() 
        {
            Updatables = new List<IUpdatable>();
            FixedUpdatables = new List<IFixedUpdatable>();
            Linkables = new List<IFixedUpdatable>();
        }

        public virtual IEnumerator LazyConstruct()
        {
            yield return null;
        }

        public virtual void Activate() { }

        public virtual void Deactivate() { }

        public virtual void Link() { }

        public virtual void PackToUpdate() { }

        public virtual void PackToFixedUpdate() { }

        protected void AddOtherUpdatables(BaseCompositeGroup compositeGroup)
        {
            compositeGroup.PackToUpdate();
            Updatables.AddRange(compositeGroup.Updatables);
        }

        protected void AddOtherFixedUpdatables(BaseCompositeGroup compositeGroup)
        {
            compositeGroup.PackToFixedUpdate();
            FixedUpdatables.AddRange(compositeGroup.FixedUpdatables);
        }
    }
}
