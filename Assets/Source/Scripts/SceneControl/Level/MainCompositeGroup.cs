using System.Collections;
using TanksArmageddon.CompositeRoot;
using TanksArmageddon.Landscape;
using TanksArmageddon.TankControl;
using UnityEngine;

namespace TanksArmageddon.SceneControl.Level
{
    public class MainCompositeGroup : BaseCompositeGroup
    {
        [SerializeField] private ColliderVisualizer _groundColliderVisualizer;
        [SerializeField] private TankSpawner _tankSpawner;

        public override void Construct()
        {
            _groundColliderVisualizer.Construct();
            _tankSpawner.Construct();
        }

        public override IEnumerator LazyConstruct()
        {
            yield return null;
        }
    }
}
