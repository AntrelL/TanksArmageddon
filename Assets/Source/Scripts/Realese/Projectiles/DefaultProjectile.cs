using System;
using TanksArmageddon;
using UnityEngine;

public class DefaultProjectile : MonoBehaviour
{
    public static Transform CurrentProjectile { get; private set; }
    public static event Action<Vector3> TankHit;
    public static event Action GroundHit;
    public static event Action ProjectileDestroyed;

    [SerializeField] private float _speed = 10f;

    private Rigidbody2D _rigidbody;
    private Cutter _cutter;
    private bool _IsDead;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        _rigidbody.velocity = transform.right * _speed;
        _cutter = FindObjectOfType<Cutter>();
        CurrentProjectile = transform;
    }

    private void Update()
    {
        transform.right = _rigidbody.velocity;

        if (transform.position.y < -50)
        {
            ProjectileDestroyed?.Invoke();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out EdgeOfMap edgeOfMap))
        {
            Debug.Log("Hit edge of map");
            ProjectileDestroyed?.Invoke();
            Destroy(gameObject);
        }

        if (collision.gameObject.TryGetComponent(out Enemy enemy))
        {
            //TankHit?.Invoke(transform.position);
            enemy.PlayHitEffect(transform.position);
            Debug.Log("Hit enemy");
            ProjectileDestroyed?.Invoke();
            Destroy(gameObject);
        }

        if (collision.gameObject.TryGetComponent(out Player player))
        {
            //TankHit?.Invoke(transform.position);
            player.PlayHitEffect(transform.position);
            Debug.Log("Hit player");
            ProjectileDestroyed?.Invoke();
            Destroy(gameObject);
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
        ProjectileDestroyed?.Invoke();
        Destroy(gameObject);
    }

    private void Delay()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        CurrentProjectile = null;
    }
}
