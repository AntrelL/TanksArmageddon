using System;
using System.Collections;
using System.Collections.Generic;

namespace TanksArmageddon.CompositeRoot.Core
{
    public abstract class BaseCompositeGroup : 
        MonoScript, IConstructable, ILazyConstructable, ILinkable
    {
        public List<IUpdatable> Updatables { get; private set; }

        public List<IFixedUpdatable> FixedUpdatables { get; private set; }

        public virtual void Construct() 
        {
            Updatables = new List<IUpdatable>();
            FixedUpdatables = new List<IFixedUpdatable>();
        }

        public virtual IEnumerator LazyConstruct()
        {
            yield return null;
        }

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

        protected void AddOtherScriptLinks(BaseCompositeGroup compositeGroup)
        {
            compositeGroup.Link();
        }

        protected void AddScriptLinks<T>(T monoScriptLinked) where T : MonoScriptLinked, ILinkable
        {
            monoScriptLinked.Link();

            foreach (var link in monoScriptLinked.Links)
            {
                RegisterSwitchableScript(monoScriptLinked, link);
            }
        }

        protected void SubscribeToSwitch<T>(
            T monoScriptLinked,
            (Action Subscribe, Action Unsubscribe) customLinkingMethods)
            where T : MonoScriptLinked, ISwitchable
        {
            monoScriptLinked.Enabled += customLinkingMethods.Subscribe ?? monoScriptLinked.Activate;
            monoScriptLinked.Disabled += customLinkingMethods.Unsubscribe ?? monoScriptLinked.Deactivate;
        }

        protected void UnsubscribeToSwitch<T>(
            T monoScriptLinked,
            (Action Subscribe, Action Unsubscribe) customLinkingMethods)
            where T : MonoScriptLinked, ISwitchable
        {
            monoScriptLinked.Enabled -= customLinkingMethods.Subscribe ?? monoScriptLinked.Activate;
            monoScriptLinked.Disabled -= customLinkingMethods.Unsubscribe ?? monoScriptLinked.Deactivate;
        }

        protected void RegisterSwitchableScript<T>(T monoScriptLinked) where T : MonoScriptLinked, ISwitchable =>
            RegisterSwitchableScript(monoScriptLinked, (null, null));

        protected void RegisterSwitchableScript<T>(
            T monoScriptLinked, 
            (Action Subscribe, Action Unsubscribe) customLinkingMethods) 
            where T : MonoScriptLinked, ISwitchable
        {
            SubscribeToSwitch(monoScriptLinked, customLinkingMethods);

            Action onDestroyed = null;
            onDestroyed = () =>
            {
                monoScriptLinked.Destroyed -= onDestroyed;
                UnsubscribeToSwitch(monoScriptLinked, customLinkingMethods);
            };

            monoScriptLinked.Destroyed += onDestroyed;
        }
    }
}
