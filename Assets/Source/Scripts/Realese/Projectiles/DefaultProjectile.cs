using System;
using TanksArmageddon;
using UnityEngine;

public class DefaultProjectile : MonoBehaviour
{
    public static Transform CurrentProjectile { get; private set; }
    public static event Action<Vector3> TankHit;
    public static event Action GroundHit;
    public static event Action ProjectileDestroyed;

    public bool IsEnemyProjectile { get; set; } = false;

    [SerializeField] private float _speed = 20f;

    private Rigidbody2D _rigidbody;
    private Cutter _cutter;
    private bool _isDead;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        _rigidbody.velocity = transform.right * _speed;
        _cutter = FindObjectOfType<Cutter>();
        CurrentProjectile = transform;
    }

    private void LateUpdate()
    {
        transform.right = _rigidbody.velocity;

        if (transform.position.y < -50)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out EdgeOfMap edgeOfMap))
        {
            Debug.Log("Hit edge of map");
            Destroy(gameObject);
        }

        if (IsEnemyProjectile)
        {
            if (collision.gameObject.TryGetComponent(out Player player))
            {
                player.PlayHitEffect(transform.position);
                Debug.Log("Hit player");
                Destroy(gameObject);
            }
        }
        else
        {
            if (collision.gameObject.TryGetComponent(out Enemy enemy))
            {
                enemy.PlayHitEffect(transform.position);
                Debug.Log("Hit enemy");
                Destroy(gameObject);
            }
        }

        if (collision.gameObject.TryGetComponent(out Land land))
        {
            _cutter.transform.position = transform.position;
            Debug.Log("Hit land");
            Invoke(nameof(DoCut), 0.001f);
        }
    }

    void DoCut()
    {
        Debug.Log("DoCut beep");
        _cutter.DoCut();
        Destroy(gameObject);
    }

    /*private void Delay()
    {
        Destroy(gameObject);
    }*/

    private void OnDestroy()
    {
        ProjectileDestroyed?.Invoke();
        CurrentProjectile = null;
    }
}
