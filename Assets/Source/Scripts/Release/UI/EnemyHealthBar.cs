using Source.Scripts.Release.Enemy;
using Source.Scripts.Release.Stuff;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Release.UI
{
    public class EnemyHealthBar : HealthBar
    {
        [SerializeField] private EnemyHealth _enemyHealth;
        [SerializeField] private GameObject _enemy;
        [SerializeField] private Vector3 _offset = new Vector3(0, 4, 0);
        [SerializeField] private Slider _healthBar;

        protected override void OnEnable()
        {
            if (_enemyHealth != null)
            {
                _enemyHealth.HealthChanged += UpdateValue;
                _enemyHealth.Defeated += DisableSlider;
            }
        }

        protected override void OnDisable()
        {
            if (_enemyHealth != null)
            {
                _enemyHealth.HealthChanged -= UpdateValue;
                _enemyHealth.Defeated -= DisableSlider;
            }
        }

        protected override int GetMaxHealth()
        {
            return _enemyHealth.MaxHealth;
        }

        private void FixedUpdate()
        {
            MoveSlider();
        }

        private void DisableSlider()
        {
            gameObject.SetActive(false);
        }

        private void MoveSlider()
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(_enemy.transform.position + _offset);
            _healthBar.transform.position = screenPosition;
        }
    }
}