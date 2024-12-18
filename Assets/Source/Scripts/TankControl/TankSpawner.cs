using Zenject;
using UnityEngine;
using System.Collections.Generic;
using TanksArmageddon.TankComponents;

namespace TanksArmageddon.TankControl
{
    public class TankSpawner : MonoBehaviour
    {
        [SerializeField] private Transform _tankStorage;
        [Space]
        [SerializeField] private Tank _playerTankPrefab;
        [SerializeField] private Transform _playerSpawnPoint;
        [Space]
        [SerializeField] private List<Tank> _enemyTankPrefabs;
        [SerializeField] private List<Transform> _enemySpawnPoints;

        private Player _player;

        [Inject]
        public void Construct(Player player)
        {
            _player = player;
            SpawnAll();
        }

        private void SpawnAll()
        {
            if (_enemyTankPrefabs.Count != _enemySpawnPoints.Count)
            {
                Debug.LogError("The number of enemy tank prefabs and spawn positions for them do not match");
                return;
            }

            SpawnTank(_playerTankPrefab, _playerSpawnPoint, _player);

            for (int i = 0; i < _enemyTankPrefabs.Count; i++)
            {
                SpawnTank(_enemyTankPrefabs[i], _enemySpawnPoints[i], new Enemy());
            }
        }

        private void SpawnTank(Tank tankPrefab, Transform spawnPoint, ITankController tankController)
        {
            Tank tank = Instantiate(tankPrefab, spawnPoint.position, Quaternion.identity, _tankStorage);
            tank.Construct(tankController);
        }
    }
}
