using UnityEngine;

namespace TanksArmageddon
{
    public class Intersection : MonoBehaviour
    {
        public static bool IsIntersecting(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
        {
            bool isIntersecting = false;
            float denominator = (p4.y - p3.y) * (p2.x - p1.x) - (p4.x - p3.x) * (p2.y - p1.y);

            if (denominator != 0)
            {
                float u_a = ((p4.x - p3.x) * (p1.y - p3.y) - (p4.y - p3.y) * (p1.x - p3.x)) / denominator;
                float u_b = ((p2.x - p1.x) * (p1.y - p3.y) - (p2.y - p1.y) * (p1.x - p3.x)) / denominator;

                if (u_a >= 0 && u_a <= 1 && u_b >= 0 && u_b <= 1)
                {
                    isIntersecting = true;
                }
            }
            
            return isIntersecting;
        }

        public static Vector2 GetIntersection(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
        {
            float top = (d.x - c.x) * (a.y - c.y) - (d.y - c.y) * (a.x - c.x);
            float bottom = (d.y - c.y) * (b.x - a.x) - (d.x - c.x) * (b.y - a.y);
            float t = top / bottom;
            Vector2 result = Vector2.Lerp(a, b, t);
            
            return result;
        }
    }
}
