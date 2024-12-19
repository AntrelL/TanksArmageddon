using UnityEngine;
using TanksArmageddon.CompositeRoot;

namespace TanksArmageddon.UI
{
    public class Bar : MonoScript, IConstructable<IReadOnlyScale>, ISwitchable, IUpdatable
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

        public void Activate()
        {
            _scale.ValueChanged += ScaleValueChanged;
        }

        public void Deactivate()
        {
            _scale.ValueChanged -= ScaleValueChanged;
        }

        public void CompositeUpdate()
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
