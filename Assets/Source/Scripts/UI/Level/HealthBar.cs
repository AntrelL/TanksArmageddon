using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Tank _tank;
    [SerializeField] private Transform _fill;
    [SerializeField] private float _maxValue;
    [SerializeField] private float _minValue;

    private float _value;

    private void OnValidate()
    {
        if (_maxValue <= _minValue)
            _maxValue = _minValue + 1;
    }

    private void Awake()
    {
        _value = _minValue;
    }

    private void OnEnable()
    {
        _tank.HealthChanged += OnHealthChanged;
    }

    private void OnDisable()
    {
        _tank.HealthChanged -= OnHealthChanged;
    }

    private void Update()
    {
        /*
        Vector2 tankPosition = _tank.transform.position;
        tankPosition.y += _verticalOffset;
        transform.position = tankPosition;
        */
    }

    private void SetValue(float value)
    {
        _value = Mathf.Clamp(value, _minValue, _maxValue);
        UpdateFillScale();
    }

    private void UpdateFillScale()
    {
        Vector3 scale = _fill.localScale;
        scale.x = _value / _maxValue;

        _fill.localScale = scale;
    }

    private void OnHealthChanged(float newValue) => SetValue(newValue);
}
