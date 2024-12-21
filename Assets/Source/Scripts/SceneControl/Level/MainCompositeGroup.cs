using System.Collections;
using TanksArmageddon.CompositeRoot;
using TanksArmageddon.Landscape;
using UnityEngine;

namespace TanksArmageddon.SceneControl.Level
{
    public class MainCompositeGroup : BaseCompositeGroup
    {
        [SerializeField] private ColliderVisualizer _groundColliderVisualizer;

        public override void Construct()
        {
            _groundColliderVisualizer.Construct();
        }

        public override IEnumerator LazyConstruct()
        {
            yield return null;
        }
    }
}
