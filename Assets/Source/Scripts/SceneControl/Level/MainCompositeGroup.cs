using UnityEngine;
using System.Collections;
using TanksArmageddon.UI;
using TanksArmageddon.Landscape;
using System.Collections.Generic;
using TanksArmageddon.TankControl;
using TanksArmageddon.TankComponents;
using TanksArmageddon.CompositeRoot.Core;

namespace TanksArmageddon.SceneControl.Level
{
    public class MainCompositeGroup : BaseCompositeGroup
    {
        [SerializeField] private ColliderVisualizer _groundColliderVisualizer;
        [SerializeField] private TankSpawner _tankSpawner;
        [SerializeField] private ButtonPressListener _moveLeftButton;
        [SerializeField] private ButtonPressListener _moveRightButton;
        [SerializeField] private Bar _fuelBar;

        private Player _player;
        private List<Tank> _tanks;

        public override void Construct()
        {
            base.Construct();

            _groundColliderVisualizer.Construct();

            _player = new Player(_moveLeftButton, _moveRightButton);
            _tanks = new List<Tank>();
            _tankSpawner.Construct((_player, _tanks));

            _fuelBar.Construct(_tanks[0].Fuel);           
        }

        public override IEnumerator LazyConstruct()
        {
            yield return null;
        }

        public override void Link()
        {
            AddScriptLinks(_fuelBar);
        }

        public override void PackToUpdate()
        {
            Updatables.AddRange(_tanks);
            Updatables.Add(_fuelBar);
        }
    }
}
