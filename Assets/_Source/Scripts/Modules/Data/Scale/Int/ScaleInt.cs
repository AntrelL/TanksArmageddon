using System;
using RainyPlace.Internal;

namespace RainyPlace
{
    public class ScaleInt : Scale<int, IReadonlyScaleInt>, IReadonlyScaleInt
    {
        public ScaleInt(int value, int min, int max, bool autoRangeLimitation = false) 
            : base(value, min, max, autoRangeLimitation)
        {
        }

        public ScaleInt(IReadonlyScaleInt sample, bool autoRangeLimitation = false) 
            : base(sample, autoRangeLimitation)
        {
        }

        protected override bool IsInRange(int value, int min, int max)
        {
            return value.IsInRange(min, max);
        }

        protected override int Clamp(int value, int min, int max)
        {
            return Math.Clamp(value, min, max);
        }
    }
}
