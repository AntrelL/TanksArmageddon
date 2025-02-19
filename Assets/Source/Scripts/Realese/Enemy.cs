using Assets.Constructors.FuturisticTanks.Scripts;
using System;
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

    private int _currentHealth;
    private bool _isAlive = true;

    private float _movementTimeUsed = 0f;
    private float _movementDirection = 0f;

    public event Action<int> HealthChanged;
    public event Action Defeated;

    private void Awake()
    {
        _currentHealth = _maxHealth;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbody2D.centerOfMass = _centerOfMass.localPosition;
    }

    private void Update()
    {
        if (!_isAlive) return;

        if (_movementTimeUsed < _availableTravelTime)
        {
            if (Input.GetKey(KeyCode.K))
            {
                _movementDirection = -1f;
                _movementTimeUsed += Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.L))
            {
                _movementDirection = 1f;
                _movementTimeUsed += Time.deltaTime;
            }
            else
            {
                _movementDirection = 0f;
            }
        }
        else
        {
            _movementDirection = 0f;
        }
    }

    private void FixedUpdate()
    {
        if (_currentHealth <= 0)
        {
            _tank.Destroy();
            _isAlive = false;
            gameObject.SetActive(false);
            return;
        }

        if (_isAlive && Mathf.Abs(_rigidbody2D.velocity.x) < _maxSpeed)
        {
            _rigidbody2D.AddForce(new Vector2(_movementDirection * _movementForce, 0f));
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

    public void PlayHitEffect(Vector3 hitPosition)
    {
        if (_isAlive)
        {
            _currentHealth -= 100;
            HealthChanged?.Invoke(_currentHealth);
            ParticleSystem flash = Instantiate(_hitFX, hitPosition, Quaternion.identity);
            flash.Play();
            Destroy(flash.gameObject, flash.main.duration);
        }
    }
}