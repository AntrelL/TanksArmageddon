using System;
using Assets.Constructors.FuturisticTanks.Scripts;
using Source.Scripts.Release.Stuff;
using UnityEngine;

namespace Source.Scripts.Release.HitProcessing
{
    public abstract class Health : MonoBehaviour
    {
        [SerializeField] private Tank _tank;
        [SerializeField] private ParticleSystem _hitFX;
        
        private int _maxHealth;
        private int _currentHealth;
        private bool _isAlive = true;
        private AudioManager _manager;
        
        public event Action<int> HealthChanged;
        
        public event Action Defeated;
        
        private void Awake()
        {
            _manager = FindObjectOfType<AudioManager>();
            _maxHealth = GetMaxHealth();
            _currentHealth = _maxHealth;
        }
        
        public void PlayHitEffect(Vector3 position)
        {
            if (_isAlive == false) 
                return;
            
            _manager.PlayTankHit();
            var flash = Instantiate(_hitFX, position, Quaternion.identity);
            flash.Play();
            Destroy(flash.gameObject, flash.main.duration);
            
            OnPlayHitEffect();
        }
        
        public void TakeDamage(int value)
        {
            if (_isAlive == false)
                return;

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

        protected virtual void OnPlayHitEffect() { }
        
        protected abstract int GetMaxHealth();
    }
}