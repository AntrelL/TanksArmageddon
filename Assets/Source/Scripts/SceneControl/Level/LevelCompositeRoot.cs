using UnityEngine;
using System.Collections;
using TanksArmageddon.CompositeRoot.Core;

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

        public override void Link()
        {
            AddOtherScriptLinks(_mainCompositeGroup);
        }

        public override void PackToUpdate()
        {
            AddOtherUpdatables(_mainCompositeGroup);
        }

        public override void PackToFixedUpdate()
        {
            AddOtherFixedUpdatables(_mainCompositeGroup);
        }
    }
}
