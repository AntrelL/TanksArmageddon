using UnityEngine;
using System.Collections.Generic;
using TanksArmageddon.CompositeRoot;
using TanksArmageddon.TankComponents;

namespace TanksArmageddon.TankControl
{
    public class TankSpawner : MonoScript, IConstructable<(Player Player, List<Tank> Tanks)>
    {
        [SerializeField] private Transform _tankStorage;
        [Space]
        [SerializeField] private Tank _playerTankPrefab;
        [SerializeField] private Transform _playerSpawnPoint;
        [Space]
        [SerializeField] private List<Tank> _enemyTankPrefabs;
        [SerializeField] private List<Transform> _enemySpawnPoints;

        private Player _player;

        private Tank _playerTank;
        private List<Tank> _enemyTanks;

        public void Construct((Player Player, List<Tank> Tanks) parameters)
        {
            _player = parameters.Player;

            SpawnAll();
            parameters.Tanks.Add(_playerTank);
            parameters.Tanks.AddRange(_enemyTanks);
        }

        private void SpawnAll()
        {
            if (_enemyTankPrefabs.Count != _enemySpawnPoints.Count)
            {
                Debug.LogError("The number of enemy tank prefabs and spawn positions for them do not match");
                return;
            }

            _playerTank = SpawnTank(_playerTankPrefab, _playerSpawnPoint, _player);
            _enemyTanks = new List<Tank>();

            for (int i = 0; i < _enemyTankPrefabs.Count; i++)
            {
                _enemyTanks.Add(SpawnTank(_enemyTankPrefabs[i], _enemySpawnPoints[i], new Enemy()));
            }
        }

        private Tank SpawnTank(Tank tankPrefab, Transform spawnPoint, ITankController tankController)
        {
            Tank tank = Instantiate(tankPrefab, spawnPoint.position, Quaternion.identity, _tankStorage);
            tank.Construct(tankController);

            return tank;
        }
    }
}