#pragma warning disable SA1202

using System.Collections.Generic;
using UnityEngine;

namespace IJunior.CompositeRoot
{
    public class FlowControl : MonoBehaviour
    {
        private List<IUpdatable> _updatableScripts;
        private List<IFixedUpdatable> _fixedUpdatableScripts;

        private bool _isRunning = false;

        private void Update()
        {
            if (_isRunning == false)
                return;

            foreach (var updatableScript in _updatableScripts)
            {
                updatableScript.OnFlowUpdate();
            }
        }

        private void FixedUpdate()
        {
            if (_isRunning == false)
                return;

            foreach (var fixedUpdatableScript in _fixedUpdatableScripts)
            {
                fixedUpdatableScript.OnFlowFixedUpdate();
            }
        }

        public void Initialize(
            List<IUpdatable> updatableScripts,
            List<IFixedUpdatable> fixedUpdatableScripts)
        {
            _updatableScripts = updatableScripts;
            _fixedUpdatableScripts = fixedUpdatableScripts;
        }

        public void Run() => _isRunning = true;

        public void Stop() => _isRunning = false;
    }
}