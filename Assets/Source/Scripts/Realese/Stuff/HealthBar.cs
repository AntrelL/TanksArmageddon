using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Release.Stuff
{
    public abstract class HealthBar : MonoBehaviour
    {
        [SerializeField] private Slider _healthSlider;
        [SerializeField] private float _smoothSpeed = 5f;
        [SerializeField] private TextMeshProUGUI _valueText;

        private int _maxHealth;
        private float _targetHealth;

        protected virtual void Awake()
        {
            _maxHealth = GetMaxHealth();
            _targetHealth = _maxHealth;
            _valueText.text = _targetHealth + "/" + _maxHealth;
            _healthSlider.maxValue = _maxHealth;
            _healthSlider.value = _maxHealth;
        }

        protected virtual void Update()
        {
            if (_healthSlider.value != _targetHealth)
            {
                _valueText.text = _targetHealth + "/" + _maxHealth;
                _healthSlider.value = Mathf.Lerp(_healthSlider.value, _targetHealth, Time.deltaTime * _smoothSpeed);
            }
        }

        protected virtual void OnEnable()
        {
        }

        protected virtual void OnDisable()
        {
        }

        protected abstract int GetMaxHealth();

        protected virtual void UpdateValue(int value)
        {
            _targetHealth = value;
        }
    }
}