using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AirdropSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _airDropPrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private PolygonCollider2D _land;
    [SerializeField] private int _spawnRate;
    private float _maxX;

    private float _minX;

    private void Start()
    {
        if (_spawnPoint == null) _spawnPoint = transform;

        CalculateWidth();
    }

    private void OnEnable()
    {
        TurnManager.CompletedTurns += CheckTurnsCount;
    }

    private void OnDisable()
    {
        TurnManager.CompletedTurns -= CheckTurnsCount;
    }

    public static event Action Spawned;

    private void CalculateWidth()
    {
        var points = _land.points;

        var minX = float.MaxValue;
        var maxX = float.MinValue;

        foreach (var point in points)
        {
            Vector2 worldPoint = _land.transform.TransformPoint(point);

            if (worldPoint.x < minX) minX = worldPoint.x;
            if (worldPoint.x > maxX) maxX = worldPoint.x;
        }

        _minX = minX;
        _maxX = maxX;
    }

    private void CheckTurnsCount(int count)
    {
        if (count % _spawnRate == 0) SpawnAirDrop();
    }

    private void SpawnAirDrop()
    {
        if (_airDropPrefab != null)
        {
            Instantiate(_airDropPrefab, _spawnPoint.position, Quaternion.identity);
            Spawned?.Invoke();
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
        var randomX = Random.Range(_minX, _maxX);
        var newPosition = _spawnPoint.position;
        newPosition.x = randomX;
        _spawnPoint.position = newPosition;

        Debug.Log($"Новая позиция SpawnPoint: X = {randomX}");
    }
}