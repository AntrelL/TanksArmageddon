using UnityEngine;
using System.Collections;

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
        }

        private IEnumerator Start()
        {
            yield return LazyConstruct();

            PackToUpdate();
            PackToFixedUpdate();
            StartFlowControl();
        }

        private void OnEnable() => Activate();

        private void OnDisable() => Deactivate();

        private void StartFlowControl()
        {
            _flowControl = GetComponent<FlowControl>();
            _flowControl.Construct((Updatables, FixedUpdatables));
            _flowControl.StartCycles();
        }
    }
}
