using System;
using UnityEngine;

namespace TanksArmageddon
{
    public class Scale : IReadOnlyScale
    {
        private float _value;

        public Scale(float startValue, float minValue, float maxValue)
        {
            if (minValue >= maxValue)
            {
                Debug.LogError("The minimum value cannot be greater than or equal to the maximum");
                return;
            }

            if (startValue.IsInRange(minValue, maxValue) == false)
            {
                Debug.LogError("The start value must be in the range of the minimum and maximum values");
                return;
            }

            MinValue = minValue;
            MaxValue = maxValue;
            Value = startValue;
        }

        public event Action<float> ValueChanged;

        public float MinValue { get; private set; }

        public float MaxValue { get; private set; }

        public float Value 
        {
            get => _value;
            set
            {
                _value = Mathf.Clamp(value, MinValue, MaxValue);
                ValueChanged?.Invoke(_value);
            }
        }

        public bool IsFull => Value == MaxValue;

        public bool IsEmpty => Value == MinValue;

        public void SetFull() => Value = MaxValue;

        public void Empty() => Value = MinValue;
    }
}
