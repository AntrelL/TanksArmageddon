using UnityEngine;

namespace TanksArmageddon.UI
{
    public class Bar : MonoBehaviour
    {
        [SerializeField][Min(0)] private float _valueChangeRate;
        [SerializeField] private Transform _fill;

        private IReadOnlyScale _scale;

        private float _value;
        private float _targetValue;

        public void Construct(IReadOnlyScale scale)
        {
            _scale = scale;
            _targetValue = _value = _scale.Value;
        }

        private void OnEnable()
        {
            _scale.ValueChanged += ScaleValueChanged;
        }

        private void OnDisable()
        {
            _scale.ValueChanged -= ScaleValueChanged;
        }

        private void Update()
        {
            _value = Mathf.MoveTowards(_value, _targetValue, _valueChangeRate * Time.deltaTime);
            _fill.localScale = _fill.localScale.SetValues(x: _value / _scale.MaxValue);
        }

        private void ScaleValueChanged(float value)
        {
            _targetValue = value;
        }
    }
}
