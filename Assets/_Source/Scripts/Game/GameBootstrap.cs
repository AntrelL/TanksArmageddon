using UnityEngine;

namespace TanksArmageddon
{
    public class GameBootstrap : MonoBehaviour
    {
        [SerializeField] private GameObject _world;
        [Space]
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private Tank _playerTank;

        private void Awake()
        {
            _playerTank.Init(_playerController);
            
            _world.SetActive(true);
        }
    }
}
