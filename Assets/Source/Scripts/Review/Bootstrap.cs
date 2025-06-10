using UnityEngine;

namespace Realese
{
    [DefaultExecutionOrder(ExecutionOrderValue)]
    public class Bootstrap : EntryPoint
    {
        [SerializeField] private PlayerTankFactory _playerFactory;

        private Tank _playerTank;

        protected override void Construct()
        {
            _playerFactory.Construct();
            _playerTank = _playerFactory.Create();
        }

        private void OnEnable()
        {
            _playerTank.Enable();
        }

        private void OnDisable()
        {
            _playerTank.Disable();
        }
    }
}
