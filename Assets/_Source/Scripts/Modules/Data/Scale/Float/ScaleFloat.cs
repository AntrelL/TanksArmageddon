using System;
using RainyPlace.Internal;

namespace RainyPlace
{
    public class ScaleFloat : Scale<float, IReadonlyScaleFloat>, IReadonlyScaleFloat
    {
        public ScaleFloat(float value, float min, float max, bool autoRangeLimitation = false) 
            : base(value, min, max, autoRangeLimitation)
        {
        }

        public ScaleFloat(IReadonlyScaleFloat sample, bool autoRangeLimitation = false) 
            : base(sample, autoRangeLimitation)
        {
        }

        protected override bool IsInRange(float value, float min, float max)
        {
            return value.IsInRange(min, max);
        }

        protected override float Clamp(float value, float min, float max)
        {
            return Math.Clamp(value, min, max);
        }
    }
}
