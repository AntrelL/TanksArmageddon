using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;

    public static event Action<Vector3> TankHit;

    private void Start()
    {
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rigidbody.velocity = transform.right * _speed;
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Enemy enemy))
        {
            TankHit?.Invoke();
            Debug.Log(collision.name);
            Destroy(gameObject);
        }
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Enemy enemy))
        {
            // Передаем позицию столкновения в событие
            TankHit?.Invoke(transform.position);
            Debug.Log("Попал в коллайдер врага!");
        }

        Destroy(gameObject); // Уничтожаем снаряд после столкновения
    }
}
