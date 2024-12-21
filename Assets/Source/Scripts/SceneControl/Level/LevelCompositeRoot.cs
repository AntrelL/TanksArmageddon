using UnityEngine;
using System.Collections;
using TanksArmageddon.CompositeRoot;

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
