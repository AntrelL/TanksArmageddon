using UnityEngine;
using System.Collections.Generic;

namespace TanksArmageddon.CompositeRoot.Core
{
    public class FlowControl : MonoScript, 
        IConstructable<(List<IUpdatable> Updatables, List<IFixedUpdatable> FixedUpdatables)>
    {
        private List<IUpdatable> _updatables;
        private List<IFixedUpdatable> _fixedUpdatables;

        public void Construct((List<IUpdatable> Updatables, List<IFixedUpdatable> FixedUpdatables) parameters)
        {
            _updatables = parameters.Updatables;
            _fixedUpdatables = parameters.FixedUpdatables;
        }

        public bool IsRunning { get; private set; } = false;

        public void StartCycles()
        {
            if (IsRunning)
            {
                Debug.LogError("The cycles have already started");
                return;
            }

            IsRunning = true;
        }

        public void StopCycles()
        {
            if (IsRunning)
            {
                Debug.LogError("The cycles have not started yet");
                return;
            }

            IsRunning = false;
        }

        private void Update()
        {
            if (IsRunning)
                _updatables.ForEach(updatable => updatable.CompositeUpdate());
        }

        private void FixedUpdate()
        {
            if (IsRunning)
                _fixedUpdatables.ForEach(fixedUpdatable => fixedUpdatable.CompositeFixedUpdate());
        }
    }
}
