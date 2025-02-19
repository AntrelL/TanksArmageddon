using System;
using TanksArmageddon;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBullet : MonoBehaviour
{
    public static event Action EnemyBulletDestroyed;

    private Cutter _cutter;

    private void Start()
    {
        _cutter = FindObjectOfType<Cutter>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out EdgeOfMap edgeOfMap))
        {
            Debug.Log("Hit edge of map");
            EnemyBulletDestroyed?.Invoke();
            Destroy(gameObject);
        }

        if (collision.gameObject.TryGetComponent(out Player player))
        {
            player.PlayHitEffect(transform.position);
            Debug.Log("Hit player");
            EnemyBulletDestroyed?.Invoke();
            Destroy(gameObject);
        }

        if (collision.gameObject.TryGetComponent(out Enemy enemy))
        {
            //enemy.PlayHitEffect(transform.position);
            Debug.Log("Enemy hit enemy");
            EnemyBulletDestroyed?.Invoke();
            Destroy(gameObject);
        }

        if (collision.gameObject.TryGetComponent(out Land land))
        {
            _cutter.transform.position = transform.position;
            Debug.Log("Hit land");
            EnemyBulletDestroyed?.Invoke();
            Invoke(nameof(DoCut), 0.001f);
        }
    }

    private void DoCut()
    {
        Debug.Log("DoCut beep");
        _cutter.DoCut();
        Destroy(gameObject);
    }
}
