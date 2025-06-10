using RainyPlace;
using UnityEngine;

namespace TanksArmageddon
{
    public class Bar : MonoBehaviour
    {
        [SerializeField] private Transform _fill;

        private IReadonlyScaleFloat _scale;

        public void Init(IReadonlyScaleFloat scale)
        {
            _scale = scale;
            SetFill(_scale.Value, _scale.Max);
        }

        private void OnEnable()
        {
            _scale.Changed += OnScaleChanged;
        }

        private void OnDisable()
        {
            _scale.Changed -= OnScaleChanged;
        }

        private void SetFill(float value, float max)
        {
            _fill.localScale = _fill.localScale.Copy(x: value / max);
        }
        
        private void OnScaleChanged((float Value, float Min, float Max) scaleValues)
        {
            SetFill(scaleValues.Value, scaleValues.Max);
        }
    }
}
