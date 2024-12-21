using System.Collections;
using TanksArmageddon.CompositeRoot;
using UnityEngine;

namespace TanksArmageddon.SceneControl.Level
{
    public class LevelCompositeRoot : BaseCompositeRoot
    {
        [SerializeField] private MainCompositeGroup _mainCompositeGroup;

        public override void Construct()
        {
            _mainCompositeGroup.Construct();
        }

        public override IEnumerator LazyConstruct()
        {
            yield return _mainCompositeGroup.LazyConstruct();
        }
    }
}
