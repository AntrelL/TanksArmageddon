using UnityEngine;

public class FuelBar : MonoBehaviour
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
        _tank.FuelChanged += OnFuelChanged;
    }

    private void OnDisable()
    {
        _tank.FuelChanged -= OnFuelChanged;
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

    private void OnFuelChanged(float newValue) => SetValue(newValue);
}
