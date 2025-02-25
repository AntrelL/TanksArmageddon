using System.Collections;
using System.Collections.Generic;
using TanksArmageddon;
using UnityEngine;

public class AirdropSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _airDropPrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private PolygonCollider2D _land;

    private float _minX = 0;
    private float _maxX = 0;

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
        TurnManager.CompletedTurns += CheckTurnsCount;
    }

    private void OnDisable()
    {
        TurnManager.CompletedTurns += CheckTurnsCount;
    }

    private void CalculateWidth()
    {
        Vector2[] points = _land.points;

        float minX = float.MaxValue;
        float maxX = float.MinValue;

        foreach (Vector2 point in points)
        {
            Vector2 worldPoint = (Vector2)_land.transform.TransformPoint(point);

            if (worldPoint.x < minX) minX = worldPoint.x;
            if (worldPoint.x > maxX) maxX = worldPoint.x;
        }

        _minX = minX;
        _maxX = maxX;
    }

    private void CheckTurnsCount(int count)
    {
        if (count % 3 == 0)
        {
            SpawnAirDrop();
        }
    }

    private void SpawnAirDrop()
    {
        if (_airDropPrefab != null)
        {
            Instantiate(_airDropPrefab, _spawnPoint.position, Quaternion.identity);
            Debug.Log("AirDrop заспавнен!");
            SetRandomSpawnPointX();
        }
        else
        {
            Debug.LogWarning("AirDropPrefab не назначен в инспекторе!");
        }
    }

    private void SetRandomSpawnPointX()
    {
        float randomX = Random.Range(_minX, _maxX);
        Vector3 newPosition = _spawnPoint.position;
        newPosition.x = randomX;
        _spawnPoint.position = newPosition;

        Debug.Log($"Новая позиция SpawnPoint: X = {randomX}");
    }
}
