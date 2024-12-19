using System;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;

namespace TanksArmageddon.CompositeRoot
{
    public class MonoScriptLinked : MonoScript, ISwitchable, ILinkable, IUpdatable, IFixedUpdatable
    {
        private List<(Action Subscribe, Action Unsubscribe)> _links = new();

        public event Action Enabled;
        public event Action Disabled;
        public event Action Destroyed;

        protected GameObject GameObjectCached { get; private set; }

        protected Transform TransformCached { get; private set; }

        public IReadOnlyList<(Action Subscribe, Action Unsubscribe)> Links => _links;

        public bool IsActivatedGameObject => GameObjectCached.activeSelf;

        private void Awake()
        {
            GameObjectCached = gameObject;
            TransformCached = transform;
        }

        private void OnEnable() => Enabled?.Invoke();

        private void OnDisable() => Disabled?.Invoke();

        private void OnDestroy() => Destroyed?.Invoke();

        protected void CreateConnection<T>(object eventObject, string eventName, T handler) where T : Delegate
        {
            EventInfo eventInfo = eventObject.GetType().GetEvent(eventName);

            MethodInfo addMethod = eventInfo.GetAddMethod();
            MethodInfo removeMethod = eventInfo.GetRemoveMethod();

            Action subscribe = () => addMethod.Invoke(eventObject, new object[] { handler });
            Action unsubscribe = () => removeMethod.Invoke(eventObject, new object[] { handler });

            _links.Add((subscribe, unsubscribe));
        }

        public virtual void Activate() { }

        public virtual void Deactivate() { }

        public virtual void Link() { }

        public virtual void CompositeUpdate() { }

        public virtual void CompositeFixedUpdate() { }
    }
}
