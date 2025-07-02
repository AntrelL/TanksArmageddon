using System;
using Assets.Constructors.FuturisticTanks.Scripts;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private ParticleSystem _hitFX;
    [SerializeField] private Tank _tank;

    private int _maxHealth;
    private int _currentHealth;
    private bool _isAlive = true;
    private AudioManager _manager;
    
    public event Action<int> HealthChanged;
    public event Action Defeated;

    private void Awake()
    {
        _manager = FindObjectOfType<AudioManager>();
        _maxHealth = PlayerDataHandler.Instance.GetPlayerMaxHealth();
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (!_isAlive) return;

        _currentHealth -= damage;
        HealthChanged?.Invoke(_currentHealth);

        if (_currentHealth <= 0)
        {
            _isAlive = false;
            _tank.Destroy();
            gameObject.SetActive(false);
            Defeated?.Invoke();
        }
    }

    public void PlayHitEffect(Vector3 position)
    {
        if (!_isAlive) return;
        
        _manager.PlayTankHit();
        var fx = Instantiate(_hitFX, position, Quaternion.identity);
        fx.Play();
        Destroy(fx.gameObject, fx.main.duration);
    }
}