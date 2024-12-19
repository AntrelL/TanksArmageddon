using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TanksArmageddon.CompositeRoot.Core
{
    [RequireComponent(typeof(FlowControl))]
    public abstract class BaseCompositeRoot : BaseCompositeGroup
    {
        private FlowControl _flowControl;

        private void Awake()
        {
            base.Construct();

            Construct();
            Link();
        }

        private IEnumerator Start()
        {
            yield return LazyConstruct();

            PackToUpdate();
            PackToFixedUpdate();
            StartFlowControl();
        }

        private void StartFlowControl()
        {
            RegisterToRemovingFromUpdateCycle(Updatables);
            RegisterToRemovingFromFixedUpdateCycle(FixedUpdatables);

            _flowControl = GetComponent<FlowControl>();
            _flowControl.Construct((Updatables, FixedUpdatables));
            _flowControl.StartCycles();
        }

        private void RegisterToRemovingFromUpdateCycle(List<IUpdatable> updatables)
        {
            foreach (var updatable in updatables)
            {
                Action onDestroyed = null;
                onDestroyed = () =>
                {
                    updatable.Destroyed -= onDestroyed;
                    
                    if (_flowControl.Updatables.Contains(updatable))
                        _flowControl.RemoveScriptFromUpdateCycle(updatable);
                };

                updatable.Destroyed += onDestroyed;
            }
        }

        private void RegisterToRemovingFromFixedUpdateCycle(List<IFixedUpdatable> fixedUpdatables)
        {
            foreach (var fixedUpdatable in fixedUpdatables)
            {
                Action onDestroyed = null;
                onDestroyed = () =>
                {
                    fixedUpdatable.Destroyed -= onDestroyed;

                    if (_flowControl.FixedUpdatables.Contains(fixedUpdatable))
                        _flowControl.RemoveScriptFromFixedUpdateCycle(fixedUpdatable);
                };

                fixedUpdatable.Destroyed += onDestroyed;
            }
        }
    }
}
