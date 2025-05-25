using TanksArmageddon;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private float _smoothSpeed = 5f;
    [SerializeField] private TextMeshProUGUI _valueText;
    [SerializeField] private Vector3 _offset = new Vector3(0, 2, 0);

    private int _maxHealth;
    private float _targetHealth;

    private void Awake()
    {
        _maxHealth = _enemy._maxHealth;
        _targetHealth = _enemy._maxHealth;
        _valueText.text = _targetHealth + "/" + _maxHealth;
        _healthSlider.maxValue = _maxHealth;
        _healthSlider.value = _maxHealth;
    }

    private void FixedUpdate()
    {
        MoveSlider();

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

        _enemy.Defeated += DisableSlider;
    }

    private void OnDisable()
    {
        if (_enemy != null)
            _enemy.HealthChanged -= UpdateValue;

        _enemy.Defeated -= DisableSlider;
    }

    private void DisableSlider()
    {
        _healthSlider.gameObject.SetActive(false);
    }

    private void MoveSlider()
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(_enemy.transform.position + _offset);
        _healthSlider.transform.position = screenPosition;
    }

    private void UpdateValue(int value)
    {
        _targetHealth = value;
    }
}
