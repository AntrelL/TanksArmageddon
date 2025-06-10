using System;

namespace RainyPlace.Internal
{
    public abstract class Scale<T, TReadonlyScale> 
        where T : IComparable<T>
        where TReadonlyScale : IReadonlyScale<T>
    {
        private readonly bool _autoRangeLimitation;
        
        private T _value;
        private T _min;
        private T _max;

        protected Scale(T value, T min, T max, bool autoRangeLimitation = false)
        {
            _autoRangeLimitation = autoRangeLimitation;
            SetValues(value, min, max);
        }

        protected Scale(TReadonlyScale sample, bool autoRangeLimitation = false) : 
            this(sample.Value, sample.Min, sample.Max, autoRangeLimitation) { }
        
        public event Action<T> ValueChanged;
        
        public event Action<(T Value, T Min, T Max)> Changed;
        
        public T Value 
        { 
            get => _value;
            set => SetValues(value, _min, _max);
        }

        public T Min
        {
            get => _min;
            set => SetValues(_value, value, _max);
        }

        public T Max
        {
            get => _max;
            set => SetValues(_value, _min, value);
        }
        
        public override string ToString() => ToString(Value, Min, Max);
        
        protected abstract T Clamp(T value, T min, T max);

        protected abstract bool IsInRange(T value, T min, T max);

        private void SetValues(T newValue, T newMin, T newMax)
        {
            bool isInRange = IsInRange(newValue, newMin, newMax);

            if (_autoRangeLimitation && isInRange == false)
            {
                newValue = Clamp(newValue, newMin, newMax);
                isInRange = true;
            }

            if (isInRange == false)
            {
                throw new Exception(
                    $"The value is out of range, {ToString(newValue, newMin, newMax)}");
            }
            
            OnValuesChanging(newValue, newMin, newMax);
            
            _value = newValue;
            _min = newMin;
            _max = newMax;
        }

        private void OnValuesChanging(T newValue, T newMin, T newMax)
        {
            bool valueChanged = newValue.CompareTo(Value) != 0;
            
            if (valueChanged)
                ValueChanged?.Invoke(newValue);

            if (valueChanged || newMin.CompareTo(Min) != 0 || newMax.CompareTo(Max) != 0)
                Changed?.Invoke((newValue, newMin, newMax));
        }
        
        private string ToString(T value, T min, T max)
        {
            return $"{nameof(Scale<T, TReadonlyScale>)}<{typeof(T).Name}> " +
                   $"Value: {value}, Range: [{min};{max}]";
        }
    }
}
