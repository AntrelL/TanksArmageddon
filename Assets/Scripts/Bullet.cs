using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static event Action<Vector3> TankHit;
    public static event Action GroundHit;

    [SerializeField] private float _speed = 10f;

    private Rigidbody2D _rigidbody;
    Cutter _cut;
    bool _dead;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        _rigidbody.velocity = transform.right * _speed;
        _cut = FindObjectOfType<Cutter>();
    }

    private void Update()
    {
        transform.right = _rigidbody.velocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Enemy enemy))
        {
            TankHit?.Invoke(transform.position);
            Debug.Log("Hit enemy");
            Destroy(gameObject);
        }

        if (collision.gameObject.TryGetComponent(out Land land))
        {
            _cut.transform.position = transform.position;
            Debug.Log("Hit land");
            Invoke(nameof(DoCut), 0.001f);
        }
    }

    void DoCut()
    {
        Debug.Log("DoCut beep");
        _cut.DoCut();
        Destroy(gameObject);
    }

    private void Delay()
    {
        Destroy(gameObject);
    }
}

