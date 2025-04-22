using System;
using UnityEngine;

namespace TanksArmageddon
{
    public static class NumberExtensions
    {
        private const string InvalidRangeError =
            "The max number of the range must be greater than the min.";

        public static bool IsInRange(this int number, int min, int max) =>
            IsInRange<int>(number, min, max);

        public static bool IsInRange(this float number, float min, float max) =>
            IsInRange<float>(number, min, max);

        private static bool IsInRange<T>(T number, T min, T max) where T : IComparable<T>
        {
            if (min.CompareTo(max) >= 0)
            {
                Debug.LogError(InvalidRangeError);
                return false;
            }

            return number.CompareTo(min) >= 0 && number.CompareTo(max) <= 0;
        }
    }
}
