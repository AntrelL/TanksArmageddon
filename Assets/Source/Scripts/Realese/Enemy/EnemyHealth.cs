using UnityEngine;
using System;
using Assets.Constructors.FuturisticTanks.Scripts;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private Tank _tank;
    [SerializeField] private ParticleSystem _hitFX;
    [SerializeField] private int _edgeOfMapDamage = 5000;

    private int _currentHealth;
    private bool _isAlive = true;

    public static event Action EnemyHitted;
    public event Action<int> HealthChanged;
    public event Action Defeated;

    public bool IsAlive => _isAlive;
    public int MaxHealth => _maxHealth;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out EdgeOfMap edge))
        {
            TakeDamage(_edgeOfMapDamage);
        }
    }

    public void TakeDamage(int value)
    {
        if (!_isAlive) return;

        _currentHealth -= value;
        HealthChanged?.Invoke(_currentHealth);

        if (_currentHealth <= 0)
        {
            _isAlive = false;
            _tank.Destroy();
            gameObject.SetActive(false);
            Defeated?.Invoke();
        }
    }

    public void PlayHitEffect(Vector3 pos)
    {
        if (!_isAlive) return;

        EnemyHitted?.Invoke();
        ParticleSystem flash = Instantiate(_hitFX, pos, Quaternion.identity);
        flash.Play();
        Destroy(flash.gameObject, flash.main.duration);
    }
}