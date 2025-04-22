using System.Collections;
using UnityEngine;

namespace TanksArmageddon
{
    [DefaultExecutionOrder(ExecutionOrderValue)]
    public class LevelBootstrap : Bootstrap
    {
        [SerializeField] private Collider2DVisualizer _groundVisualizer;
        [SerializeField] private Tank _testTank;

        [SerializeField] private PlayerControls _playerControls;
        
        public override IEnumerator Initialize()
        {
            _groundVisualizer.Initialize();
            _testTank.Initialize(new Player(_playerControls));

            return null;
        }
    }
}
