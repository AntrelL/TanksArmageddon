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
        [Space]
        [SerializeField] private ButtonPressListener _moveLeftButton;
        [SerializeField] private ButtonPressListener _moveRightButton;
        [SerializeField] private Bar _fuelBar;

        public override void Construct()
        {
            _groundColliderVisualizer.Construct();

            _tankSpawner.Construct(new Player(_moveLeftButton, _moveRightButton));
            _fuelBar.Construct(_tankSpawner.PlayerTank.Fuel);
        }

        public override IEnumerator LazyConstruct()
        {
            yield return null;
        }
    }
}
