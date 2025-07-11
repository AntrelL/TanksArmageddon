using System;
using Source.Scripts.Release.TurnManager;
using UnityEngine;

namespace Source.Scripts.Release.Airdrop
{
    public class AirdropSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _airDropPrefab;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private PolygonCollider2D _land;
        [SerializeField] private int _spawnRate;
        [SerializeField] private TurnState _turnState;

        private float _minX = 0;
        private float _maxX = 0;

        public event Action Spawned;

        private void Start()
        {
            if (_spawnPoint == null)
            {
                _spawnPoint = this.transform;
            }

            CalculateWidth();
        }

        private void OnEnable()
        {
            _turnState.CompletedTurns += OnCompletedTurns;
        }

        private void OnDisable()
        {
            _turnState.CompletedTurns -= OnCompletedTurns;
        }

        private void CalculateWidth()
        {
            Vector2[] points = _land.points;

            float minX = float.MaxValue;
            float maxX = float.MinValue;

            foreach (Vector2 point in points)
            {
                Vector2 worldPoint = (Vector2)_land.transform.TransformPoint(point);

                if (worldPoint.x < minX)
                    minX = worldPoint.x;

                if (worldPoint.x > maxX)
                    maxX = worldPoint.x;
            }

            _minX = minX;
            _maxX = maxX;
        }

        private bool TrySpawn(int turnsCount)
        {
            if (turnsCount % _spawnRate == 0)
            {
                SpawnAirDrop();
                return true;
            }

            return false;
        }

        private void SpawnAirDrop()
        {
            if (_airDropPrefab != null)
            {
                Instantiate(_airDropPrefab, _spawnPoint.position, Quaternion.identity);
                Spawned?.Invoke();
                SetRandomSpawnPointX();
            }
        }

        private void SetRandomSpawnPointX()
        {
            float randomX = UnityEngine.Random.Range(_minX, _maxX);
            Vector3 newPosition = _spawnPoint.position;
            newPosition.x = randomX;
            _spawnPoint.position = newPosition;
        }

        private void OnCompletedTurns(int count) => TrySpawn(count);
    }
}
