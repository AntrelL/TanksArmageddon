using TanksArmageddon;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private int _maxHealth = 1000;
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private float _smoothSpeed = 5f;
    [SerializeField] private TextMeshProUGUI _valueText;

    private float _targetHealth;

    private void Awake()
    {
        _targetHealth = _maxHealth;
        _valueText.text = _targetHealth + "/" + _maxHealth;
        _healthSlider.maxValue = _maxHealth;
        _healthSlider.value = 1000;
    }

    private void Update()
    {
        if (_healthSlider.value != _targetHealth)
        {
            _valueText.text = _targetHealth + "/" + _maxHealth;
            _healthSlider.value = Mathf.Lerp(_healthSlider.value, _targetHealth, Time.deltaTime * _smoothSpeed);
        }
    }

    private void OnEnable()
    {
        if (_enemy != null)
            _enemy.HealthChanged += UpdateValue;
    }

    private void OnDisable()
    {
        if (_enemy != null)
            _enemy.HealthChanged -= UpdateValue;
    }

    private void UpdateValue(int value)
    {
        _targetHealth = value;
    }
}
