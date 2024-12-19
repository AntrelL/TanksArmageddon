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

        public IReadOnlyList<IUpdatable> Updatables => _updatables;

        public IReadOnlyList<IFixedUpdatable> FixedUpdatables => _fixedUpdatables;

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
            if (IsRunning == false)
                return;

            foreach (var updatable in _updatables)
            {
                if (IsCorrectForUpdate(updatable) == false)
                    continue;

                updatable.CompositeUpdate();
            }
        }

        private void FixedUpdate()
        {
            if (IsRunning == false)
                return;

            foreach (var fixedUpdatable in _fixedUpdatables)
            {
                if (IsCorrectForUpdate(fixedUpdatable) == false)
                    continue;

                fixedUpdatable.CompositeFixedUpdate();
            }
        }

        public void AddScriptToUpdateCycle(IUpdatable updatable) =>
            AddToCycleList(_updatables, updatable, "update");

        public void RemoveScriptFromUpdateCycle(IUpdatable updatable) =>
            RemoveFromCycleList(_updatables, updatable, "update");

        public void AddScriptToFixedUpdateCycle(IFixedUpdatable fixedUpdatable) =>
            AddToCycleList(_fixedUpdatables, fixedUpdatable, "fixed update");

        public void RemoveScriptFromFixedUpdateCycle(IFixedUpdatable fixedUpdatable) =>
            RemoveFromCycleList(_fixedUpdatables, fixedUpdatable, "fixed update");

        private void AddToCycleList<T>(List<T> cycleList, T element, string cycleName)
        {
            if (cycleList.Contains(element))
            {
                Debug.LogError($"This script is already in the {cycleName} cycle");
                return;
            }

            cycleList.Add(element);
        }

        private void RemoveFromCycleList<T>(List<T> cycleList, T element, string cycleName)
        {
            if (cycleList.Contains(element) == false)
            {
                Debug.LogError($"There is no such script in the {cycleName} cycle");
                return;
            }

            cycleList.Remove(element);
        }

        private bool IsCorrectForUpdate<T>(T updatableObject) where T : IActivatableGameObject, IEnableableComponent
        {
            return updatableObject != null && updatableObject.IsActivatedGameObject && updatableObject.enabled;
        }
    }
}
