using Assets.Constructors.FuturisticTanks.Scripts;
using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private ParticleSystem _hitFX;
    [SerializeField] private int _maxHealth = 1000;
    [SerializeField] private Tank _tank;
    [SerializeField] private float _movementForce = 15f;
    [SerializeField] private float _maxSpeed = 5f;
    [SerializeField] private Transform _centerOfMass;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private float _availableTravelTime = 10f;
    [SerializeField] private ProjectileShooter2D _projectileShooter;
    [SerializeField] private Transform _player;
    [SerializeField] private LayerMask _landLayer;

    private int _currentHealth;
    private bool _isAlive = true;
    private int _playerDamage = 100;

    private float _movementTimeUsed = 0f;
    private float _moveDirection = 0f;

    public event Action<int> HealthChanged;
    public event Action Defeated;
    public static event Action EnemyHitted;

    private void Awake()
    {
        _currentHealth = _maxHealth;

        if (!_rigidbody2D)
            _rigidbody2D = GetComponent<Rigidbody2D>();

        _rigidbody2D.centerOfMass = _centerOfMass.localPosition;
    }

    private void OnEnable()
    {
        DefaultProjectile.TankHit += PlayHitEffect;
        InventoryManager.UpdatePlayerDamage += OnUpdatedPlayerDamage;
    }

    private void OnDisable()
    {
        DefaultProjectile.TankHit -= PlayHitEffect;
        InventoryManager.UpdatePlayerDamage -= OnUpdatedPlayerDamage;
    }

    private void FixedUpdate()
    {
        if (!_isAlive)
            return;

        if (_currentHealth <= 0)
        {
            _tank.Destroy();
            _isAlive = false;
            gameObject.SetActive(false);
            Defeated?.Invoke();
            return;
        }

        /*if (_movementTimeUsed < _availableTravelTime && _moveDirection != 0f)
        {
            if (Mathf.Abs(_rigidbody2D.velocity.x) < _maxSpeed)
            {
                _tank.Move();
                _rigidbody2D.AddForce(new Vector2(_moveDirection * _movementForce, 0f));
            }

            _movementTimeUsed += Time.fixedDeltaTime;
        }
        else
        {

            _moveDirection = 0f;
        }*/

        if (_movementTimeUsed < _availableTravelTime && _moveDirection != 0f)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, 1f, _landLayer);

            if (hit.collider != null)
            {
                Vector2 direction = Vector2.right * _moveDirection;
                direction = direction - (Vector2.Dot(direction, hit.normal) * hit.normal);

                _rigidbody2D.AddForce(direction * _movementForce);
            }
            else
            {
                Debug.Log("hit collider == null");
            }

            if (_rigidbody2D.velocity.magnitude > _maxSpeed)
                _rigidbody2D.velocity = _rigidbody2D.velocity.normalized * _maxSpeed;

            _tank.Move();
            _movementTimeUsed += Time.fixedDeltaTime;
        }
        else
        {
            _moveDirection = 0f;
            _tank.Idle();
        }
    }

    private void OnDrawGizmos()
    {
        // Отрисовываем рейкаст, как он используется в FixedUpdate
        Gizmos.color = Color.red;
        Vector2 rayDirection = -Vector2.up;
        float rayLength = 1f;
        Gizmos.DrawRay(transform.position, rayDirection * rayLength);

        // Выполняем рейкаст для определения точки столкновения
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, rayLength, _landLayer);

        if (hit.collider != null)
        {
            // Отрисовываем точку столкновения зелёным шариком
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(hit.point, 0.1f);
        }
    }

    public IEnumerator DoEnemyTurn()
    {
        _movementTimeUsed = 0f;

        bool shotSucceeded = _projectileShooter.ShootIfPossible();

        if (shotSucceeded)
        {
            yield return WaitProjectileFly();
            yield break;
        }

        Debug.Log($"Враг {name}: нет баллистического решения — начинаю двигаться к игроку.");

        _moveDirection = -1f;

        float elapsed = 0f;
        float checkInterval = 0.5f;

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
        bool projectileEnded = false;
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
            ParticleSystem flash = Instantiate(_hitFX, hitPosition, Quaternion.identity);
            flash.Play();
            Destroy(flash.gameObject, flash.main.duration);
        }
    }
}
