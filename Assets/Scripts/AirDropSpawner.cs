using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirDropSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _airDropPrefab; // Префаб AirDrop
    [SerializeField] private Transform _spawnPoint; // Точка спавна AirDrop
    [SerializeField] private int _turnsToSpawn = 5; // Количество ходов до спавна AirDrop
    [SerializeField] private PolygonCollider2D _land;

    private int _currentTurnCount = 0; // Текущий счетчик ходов
    private float _minX = 0;
    private float _maxX = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnAirDrop();
        }
    }

    private void Start()
    {
        // Инициализация, если точка спавна не указана
        if (_spawnPoint == null)
        {
            _spawnPoint = this.transform; // Спавнить на позиции спавнера
        }

        CalculateWidth();
    }

    private void CalculateWidth()
    {
        // Получаем все точки коллайдера (основной путь - 0)
        Vector2[] points = _land.points;

        // Минимальные и максимальные значения по X
        float minX = float.MaxValue;
        float maxX = float.MinValue;

        foreach (Vector2 point in points)
        {
            // Переводим локальные координаты точек в мировые
            Vector2 worldPoint = (Vector2)_land.transform.TransformPoint(point);

            // Находим минимальный и максимальный X
            if (worldPoint.x < minX) minX = worldPoint.x;
            if (worldPoint.x > maxX) maxX = worldPoint.x;
        }

        _minX = minX;
        _maxX = maxX;
    }

    private void IncrementTurn()
    {
        _currentTurnCount++;

        // Если достигли нужного количества ходов — спавним AirDrop
        if (_currentTurnCount >= _turnsToSpawn)
        {
            SpawnAirDrop();
            _currentTurnCount = 0; // Сбрасываем счетчик
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
