using System.Collections;
using UnityEngine;

namespace TanksArmageddon
{
    [DefaultExecutionOrder(ExecutionOrderValue)]
    public class LevelBootstrap : Bootstrap
    {
        [SerializeField] private Collider2DVisualizer _groundVisualizer;

        public override IEnumerator Initialize()
        {
            _groundVisualizer.Initialize();

            return null;
        }
    }
}
