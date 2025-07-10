using System.Collections.Generic;

namespace Source.Scripts.Release.Utils
{
    public static class ListExtensions
    {
        public static void RotateLeft<T>(this List<T> list)
        {
            if (list == null || list.Count < 2)
                return;

            T first = list[0];

            for (int i = 0; i < list.Count - 1; i++)
                list[i] = list[i + 1];

            list[^1] = first;
        }
    }
}