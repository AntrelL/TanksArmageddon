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
    [SerializeField] private EnemyTurretController _turretController;

    private int _currentHealth;
    private bool _isAlive = true;
    private Rigidbody2D _rigidbody;

    public EnemyTurretController TurretController => _turretController;

    public event Action<int> HealthChanged;
    public event Action Defeated;

    private void Awake()
    {
        _currentHealth = _maxHealth;
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.centerOfMass = _centerOfMass.localPosition;
    }

    private void FixedUpdate()
    {
        if (_currentHealth <= 0)
        {
            _tank.Destroy();
            _isAlive = false;
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        DefaultProjectile.TankHit += PlayHitEffect;
    }

    private void OnDisable()
    {
        DefaultProjectile.TankHit -= PlayHitEffect;
    }

    private void TakeDamage(int damage)
    {
        _currentHealth -= damage;
    }

    public void PlayHitEffect(Vector3 hitPosition)
    {
        if (_isAlive == true)
        {
            _currentHealth -= 100;
            HealthChanged?.Invoke(_currentHealth);
            ParticleSystem flash = Instantiate(_hitFX, hitPosition, Quaternion.identity);
            flash.Play();
            Destroy(flash.gameObject, flash.main.duration);
        }
        else
        {
            return;
        }
    }

    public IEnumerator MoveAndCheckShooting(Transform target, float availableMovementTime, float difficultyFactor)
    {
        float timer = 0f;
        bool canShoot = false;

        while (timer < availableMovementTime)
        {
            float horizontalInput = Mathf.Sign(target.position.x - transform.position.x);

            if (Mathf.Abs(horizontalInput) > 0.001f)
            {
                _tank.Move();
            }
            else
            {
                _tank.Idle();
            }

            Vector2 forceDirection = new Vector2(horizontalInput, 0f);
            _rigidbody.AddForce(forceDirection * _movementForce);

            if (_rigidbody.velocity.magnitude > _maxSpeed)
            {
                _rigidbody.velocity = _rigidbody.velocity.normalized * _maxSpeed;
            }

            if (timer % 0.5f < Time.deltaTime)
            {
                canShoot = _turretController.CanShoot(target, difficultyFactor);

                if (canShoot) break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        _turretController.Shoot(target, difficultyFactor);
    }
}
