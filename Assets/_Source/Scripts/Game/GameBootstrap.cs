using UnityEngine;

namespace TanksArmageddon
{
    public class GameBootstrap : MonoBehaviour
    {
        [SerializeField] private GameObject _world;
        [Space]
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private Tank _playerTank;
        [Space] 
        [SerializeField] private Bar _fuelBar;

        private void Awake()
        {
            _playerTank.Init(_playerController);
            _fuelBar.Init(_playerTank.Fuel);
        }

        private void Start()
        {
            _world.SetActive(true);
        }
    }
}
