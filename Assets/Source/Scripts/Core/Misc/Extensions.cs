using System;
using UnityEngine;

namespace TanksArmageddon.Core
{
    public static class Extensions
    {
        public static Vector3 SetValues(
            this Vector3 vector, float x = float.NaN, float y = float.NaN, float z = float.NaN)
        {
            return new Vector3 (
                SelectCorrectValue(x, vector.x), 
                SelectCorrectValue(y, vector.y),
                SelectCorrectValue(z, vector.z)
            );
        }

        public static Vector2 SetValues(
            this Vector2 vector, float x = float.NaN, float y = float.NaN)
        {
            return new Vector2(
                SelectCorrectValue(x, vector.x),
                SelectCorrectValue(y, vector.y)
            );
        }

        public static bool IsInRange(this int number, int min, int max) => IsInRange<int>(number, min, max);

        public static bool IsInRange(this float number, float min, float max) => IsInRange<float>(number, min, max);

        private static bool IsInRange<T>(T number, T min, T max) where T : IComparable<T>
        {
            return number.CompareTo(min) >= 0 && number.CompareTo(max) <= 0;
        }

        private static float SelectCorrectValue(float newValue, float defaultValue)
        {
            return float.IsNaN(newValue) ? defaultValue : newValue;
        }
    }
}
