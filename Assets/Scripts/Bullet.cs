using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static event Action<Vector3> TankHit;
    public static event Action GroundHit;

    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _explosionRadius = 3f;

    private Rigidbody2D _rigidbody;
    private TerrainDestruction _terrainDestruction;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        _rigidbody.velocity = transform.right * _speed;

        _terrainDestruction = FindObjectOfType<TerrainDestruction>();
    }

    private void Update()
    {
        transform.right = _rigidbody.velocity;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Enemy enemy))
        {
            // Передаем позицию столкновения в событие
            TankHit?.Invoke(transform.position);
            Debug.Log("Попал в коллайдер врага!");
        }
        
        {

        }

            /*if (collision.gameObject.TryGetComponent(out TerrainDestruction terrain))
            {
                //Vector2 hitPosition = transform.position;  // Позиция попадания снаряда

                // Разрушаем ландшафт
                _terrainDestruction.DestroyTerrain(transform.position, _explosionRadius);
                Debug.Log("!разрушили ландшафт!");
            }*/


            Destroy(gameObject); // Уничтожаем снаряд после столкновения
        }
    }

    /*private void OnTriggerEnter2D(Collision2D collision)
    {
        // Проверяем, что столкнулись с ландшафтом
        if (collision.gameObject.TryGetComponent(out TerrainDestruction terrain))
        {
            Vector2 hitPosition = transform.position;  // Позиция попадания снаряда

            // Разрушаем ландшафт
            _terrainDestruction.DestroyTerrain(hitPosition, _explosionRadius);
            Debug.Log("!разрушили ландшафт!");
            // Уничтожаем снаряд после попадания
            Destroy(gameObject);
        }
    }*/

