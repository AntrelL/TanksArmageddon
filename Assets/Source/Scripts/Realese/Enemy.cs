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

    private int _currentHealth;
    private bool _isAlive = true;

    public event Action<int> HealthChanged;
    public event Action Defeated;

    private void Awake()
    {
        _currentHealth = _maxHealth;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbody2D.centerOfMass = _centerOfMass.localPosition;
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
}
