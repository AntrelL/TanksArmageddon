using TanksArmageddon;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private float _smoothSpeed = 5f;
    [SerializeField] private TextMeshProUGUI _valueText;

    private int _maxHealth;
    private float _targetHealth;

    private void Awake()
    {
        _maxHealth = GameManager.Instance.GetPlayerMaxHealth();
        _targetHealth = _maxHealth;
        _valueText.text = _targetHealth + "/" + _maxHealth;
        _healthSlider.maxValue = _maxHealth;
        _healthSlider.value = _maxHealth;
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
        if (_player != null)
            _player.HealthChanged += UpdateValue;
    }

    private void OnDisable()
    {
        if (_player != null)
            _player.HealthChanged -= UpdateValue;
    }

    private void UpdateValue(int value)
    {
        _targetHealth = value;
    }
}
