using Assets.Constructors.FuturisticTanks.Scripts;
using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private ParticleSystem _hitFX;
    [SerializeField] private int _maxHealth = 1000;
    [SerializeField] private Tank _tank;

    private int _currentHealth;
    private bool _isAlive = true;

    public event Action<int> HealthChanged;
    public event Action Defeated;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    private void FixedUpdate()
    {
        if (_currentHealth <= 0)
        {
            Defeated?.Invoke();
            _tank.Destroy();
            _isAlive = false;
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
}
