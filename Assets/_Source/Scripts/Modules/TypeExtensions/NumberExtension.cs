using System;

namespace RainyPlace
{
    public static class NumberExtensions
    {
        public static bool ApproximatelyEquals(this float a, float b, float tolerance = 1e-6f)
        {
            return Math.Abs(a - b) < tolerance;
        }
        
        public static bool IsInRange(this int number, int min, int max) =>
            IsInRange<int>(number, min, max);

        public static bool IsInRange(this float number, float min, float max) =>
            IsInRange<float>(number, min, max);

        private static bool IsInRange<T>(T number, T min, T max) where T : IComparable<T>
        {
            // TODO: Adapt to work with float
            if (min.CompareTo(max) >= 0)
            {
                throw new Exception(
                    "The max number of the range must be greater than the min");
            }
            
            return number.CompareTo(min) >= 0 && number.CompareTo(max) <= 0;
        }
    }
}
