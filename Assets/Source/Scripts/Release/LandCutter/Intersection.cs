using UnityEngine;

namespace Source.Scripts.Release.LandCutter
{
    public class Intersection
    {
        public static bool IsIntersecting(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
        {
            bool isIntersecting = false;
            float denominator = ((p4.y - p3.y) * (p2.x - p1.x)) - ((p4.x - p3.x) * (p2.y - p1.y));

            if (denominator != 0)
            {
                float u_a = (((p4.x - p3.x) * (p1.y - p3.y)) - ((p4.y - p3.y) * (p1.x - p3.x))) / denominator;
                float u_b = (((p2.x - p1.x) * (p1.y - p3.y)) - ((p2.y - p1.y) * (p1.x - p3.x))) / denominator;

                if (u_a >= 0 && u_a <= 1 && u_b >= 0 && u_b <= 1)
                {
                    isIntersecting = true;
                }
            }

            return isIntersecting;
        }

        public static Vector2 GetIntersection(Vector2 start1, Vector2 end1, Vector2 start2, Vector2 end2)
        {
            float top = ((end2.x - start2.x) * (start1.y - start2.y)) - ((end2.y - start2.y) * (start1.x - start2.x));
            float bottom = ((end2.y - start2.y) * (end1.x - start1.x)) - ((end2.x - start2.x) * (end1.y - start1.y));
            float t = top / bottom;
            Vector2 result = Vector2.Lerp(start1, end1, t);

            return result;
        }
    }
}
