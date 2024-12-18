using System;

namespace TanksArmageddon
{
    public static class Extensions
    {
        public static bool IsInRange(this int number, int min, int max) => IsInRange<int>(number, min, max);

        public static bool IsInRange(this float number, float min, float max) => IsInRange<float>(number, min, max);

        private static bool IsInRange<T>(T number, T min, T max) where T : IComparable<T>
        {
            return number.CompareTo(min) >= 0 && number.CompareTo(max) <= 0;
        }
    }
}
