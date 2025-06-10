using UnityEngine;

namespace Realese
{
    internal class PlayerTankFactory : TankFactory
    {
        [SerializeField] private PlayerControls _playerControls;

        private PlayerController _playerController;

        public override Tank Create()
        {
            Tank playerTank = new(
            _playerController,
            new ScaleInt(1000, 0, 1000));

            return playerTank;
        }

        public void Construct()
        {
            _playerController = new PlayerController(_playerControls);
            OnConstructed();
        }

        protected override void OnActivate()
        {
            _playerController.Enable();
        }

        protected override void OnDeactivate()
        {
            _playerController.Disable();
        }
    }
}
