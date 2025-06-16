using System;
using System.Collections;
using Assets.Constructors.FuturisticTanks.Scripts;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private ParticleSystem _hitFX;
    [SerializeField] public int _maxHealth;
    [SerializeField] private Tank _tank;
    [SerializeField] private float _movementForce = 15f;
    [SerializeField] private float _maxSpeed = 5f;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private float _availableTravelTime = 3f;
    [SerializeField] private ProjectileShooter2D _projectileShooter;
    [SerializeField] private Transform _player;
    [SerializeField] private LayerMask _landLayer;

    [Space] [Header("New Physics")] [SerializeField]
    private Transform _centerPoint;

    [SerializeField] private Transform _leftPoint;
    [SerializeField] private Transform _rightPoint;

    private float _baseDrag;
    private readonly float _checkRaycastLenght = 0.8f;

    private int _currentHealth;
    private Vector3 _forceDirection;
    private bool _isAlive = true;
    private float _moveDirection;

    private float _movementTimeUsed;
    private int _playerDamage = 100;

    private Vector3 _selectedPointPosition;

    private void Awake()
    {
        _currentHealth = _maxHealth;

        if (!_rigidbody2D)
            _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (_currentHealth <= 0)
        {
            _tank.Destroy();
            _isAlive = false;
            gameObject.SetActive(false);
            Defeated?.Invoke();

            return;
        }

        if (_movementTimeUsed >= _availableTravelTime || _moveDirection == 0f)
        {
            _rigidbody2D.drag = 100f;
            _tank.Idle();

            return;
        }

        _rigidbody2D.centerOfMass = _centerPoint.localPosition;

        _rigidbody2D.drag = _baseDrag;

        _selectedPointPosition = _moveDirection == 1f ? _rightPoint.position : _leftPoint.position;
        var hit = Physics2D.Raycast(_selectedPointPosition, -Vector2.up, _checkRaycastLenght, _landLayer);

        if (hit.collider == null)
        {
            _rigidbody2D.AddForceAtPosition(_moveDirection * Vector2.right * _movementForce, _selectedPointPosition);
            _rigidbody2D.gravityScale = 10f;
            hit = Physics2D.Raycast(_centerPoint.position, -Vector2.up, _checkRaycastLenght, _landLayer);
        }

        var direction = new Vector2();

        if (hit.collider != null)
        {
            _rigidbody2D.gravityScale = 1f;
            direction = Vector2.right * _moveDirection;
            direction = direction - Vector2.Dot(direction, hit.normal) * hit.normal;
        }
        else
        {
            direction = transform.right * _moveDirection;
            _selectedPointPosition = transform.position;
        }

        _forceDirection = direction.normalized;
        _rigidbody2D.AddForceAtPosition(_forceDirection * _movementForce, _selectedPointPosition);

        if (_rigidbody2D.velocity.magnitude > _maxSpeed)
            _rigidbody2D.velocity = _rigidbody2D.velocity.normalized * _maxSpeed;

        _tank.Move();
        _movementTimeUsed += Time.fixedDeltaTime;
    }

    private void OnEnable()
    {
        InventoryManager.UpdatePlayerDamage += OnUpdatedPlayerDamage;
    }

    private void OnDisable()
    {
        InventoryManager.UpdatePlayerDamage -= OnUpdatedPlayerDamage;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out EdgeOfMap edge))
        {
            Debug.Log("Enemy hit edge of map");
            TakeDamage(5000);
        }
    }

    public event Action<int> HealthChanged;
    public event Action Defeated;
    public static event Action EnemyHitted;


    public IEnumerator DoEnemyTurn()
    {
        _movementTimeUsed = 0f;

        var shotSucceeded = _projectileShooter.ShootIfPossible();

        if (shotSucceeded)
        {
            yield return WaitProjectileFly();
            yield break;
        }

        Debug.Log($"Враг {name}: нет баллистического решения — начинаю двигаться к игроку.");

        _moveDirection = -1f;

        var elapsed = 0f;
        var checkInterval = 0.1f;

        while (elapsed < _availableTravelTime)
        {
            yield return new WaitForSeconds(checkInterval);
            elapsed += checkInterval;

            shotSucceeded = _projectileShooter.ShootIfPossible();

            if (shotSucceeded)
            {
                _moveDirection = 0f;
                yield return WaitProjectileFly();
                yield break;
            }
        }

        _moveDirection = 0f;
        Debug.Log($"Враг {name} завершил ход после движения и не может попасть в игрока.");
    }

    private IEnumerator WaitProjectileFly()
    {
        var projectileEnded = false;
        Action onProjectileDestroyed = () => { projectileEnded = true; };
        EnemyBullet.EnemyBulletDestroyed += onProjectileDestroyed;

        yield return new WaitUntil(() => projectileEnded);
        EnemyBullet.EnemyBulletDestroyed -= onProjectileDestroyed;
    }

    private void OnUpdatedPlayerDamage(int value)
    {
        _playerDamage = value;
    }

    private void TakeDamage(int value)
    {
        _currentHealth -= value;
    }

    public void PlayHitEffect(Vector3 hitPosition)
    {
        if (_isAlive)
        {
            EnemyHitted?.Invoke();
            TakeDamage(_playerDamage);
            HealthChanged?.Invoke(_currentHealth);
            var flash = Instantiate(_hitFX, hitPosition, Quaternion.identity);
            flash.Play();
            Destroy(flash.gameObject, flash.main.duration);
        }
    }
}