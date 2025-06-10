using UnityEngine;

namespace RainyPlace
{
    public static class VectorExtensions
    {
        public static Vector3 Copy(
            this Vector3 vector, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(
                Select(x, vector.x),
                Select(y, vector.y),
                Select(z, vector.z)
            );
        }

        public static Vector2 Copy(
            this Vector2 vector, float? x = null, float? y = null)
        {
            return new Vector2(
                Select(x, vector.x),
                Select(y, vector.y)
            );
        }

        private static float Select(float? newValue, float defaultValue)
        {
            return newValue ?? defaultValue;
        }
    }
}
