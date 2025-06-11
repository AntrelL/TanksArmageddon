using System;

namespace RainyPlace
{
    public static class NumberExtensions
    {
        private const float DefaultTolerance = 1e-6f;
        
        public static bool ApproximatelyEquals(
            this float a, float b, float tolerance = DefaultTolerance)
        {
            return Math.Abs(a - b) < tolerance;
        }
        
        private static int ApproximatelyCompareTo(
            this float a, float b, float tolerance = DefaultTolerance)
        {
            return (a - b).ApproximatelyEquals(0) ? 0 : a.CompareTo(b);
        }
        
        public static bool IsInRange(this int number, int min, int max) =>
            IsInRange<int>(number, min, max, (int a, int b) => a.CompareTo(b));

        public static bool IsInRange(this float number, float min, float max) =>
            IsInRange<float>(number, min, max, (float a, float b) => a.ApproximatelyCompareTo(b));

        private static bool IsInRange<T>(T number, T min, T max, Func<T, T, int> comparator)
        {
            if (comparator.Invoke(min, max) >= 0)
                throw new Exception("The max number of the range must be greater than the min");
            
            return comparator.Invoke(number, min) >= 0 && comparator.Invoke(number, max) <= 0;
        }
    }
}
