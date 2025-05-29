using System;

namespace RainyPlace
{
    public class ScaleFloat : Scale<float>
    {
        public ScaleFloat(float value, float min, float max, bool autoRangeLimitation = false) 
            : base(value, min, max, autoRangeLimitation) { }

        public ScaleFloat(IReadOnlyScale<float> sample, bool autoRangeLimitation = false) 
            : base(sample, autoRangeLimitation) { }

        protected override bool IsInRange(float value, float min, float max) => value.IsInRange(min, max);

        protected override float Clamp(float value, float min, float max) => Math.Clamp(value, min, max);
    }
}
