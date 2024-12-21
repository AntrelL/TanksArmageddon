using System.Collections;
using TanksArmageddon.CompositeRoot;
using TanksArmageddon.Landscape;
using TanksArmageddon.TankControl;
using TanksArmageddon.UI;
using UnityEngine;

namespace TanksArmageddon.SceneControl.Level
{
    public class MainCompositeGroup : BaseCompositeGroup
    {
        [SerializeField] private ColliderVisualizer _groundColliderVisualizer;
        [Space]
        [SerializeField] private TankSpawner _tankSpawner;
        [SerializeField] private ButtonPressListener _moveLeftButton;
        [SerializeField] private ButtonPressListener _moveRightButton;

        private Player _player;

        public override void Construct()
        {
            _groundColliderVisualizer.Construct();

            _player = new Player(_moveLeftButton, _moveRightButton);
            _tankSpawner.Construct(_player);
        }

        public override IEnumerator LazyConstruct()
        {
            yield return null;
        }
    }
}
