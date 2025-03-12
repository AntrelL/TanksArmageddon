using System;
using TanksArmageddon;
using UnityEngine;

public class DefaultProjectile : MonoBehaviour
{
    [SerializeField] private ParticleSystem _groundCollisionFX;

    public static Transform CurrentProjectile { get; private set; }
    public static event Action GroundHit;
    public static event Action EdgeOfMapHit;
    public static event Action ProjectileDestroyed;

    public float Speed => _speed;
    public bool IsEnemyProjectile { get; set; } = false;

    [SerializeField] private float _speed;

    private Rigidbody2D _rigidbody;
    private Cutter _cutter;
    private bool _isDead;

    private float _targetX;
    private float _targetY;

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

    public void SetupBallisticTrajectory(float targetX, float targetY)
    {
        _targetX = targetX;
        _targetY = targetY;

        Vector2 direction = new Vector2(_targetX - transform.position.x, _targetY - transform.position.y);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _rigidbody.velocity = new Vector2(Mathf.Cos(angle) * _speed, Mathf.Sin(angle) * _speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out EdgeOfMap edgeOfMap))
        {
            Debug.Log("Hit edge of map");
            EdgeOfMapHit?.Invoke();
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

    private void DoCut()
    {
        Debug.Log("DoCut beep");
        ParticleSystem flash = Instantiate(_groundCollisionFX, transform.position, transform.rotation);
        flash.Play();
        Destroy(flash.gameObject, flash.main.duration);

        _cutter.DoCut();
        GroundHit?.Invoke();
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        ProjectileDestroyed?.Invoke();
        CurrentProjectile = null;
    }
}
