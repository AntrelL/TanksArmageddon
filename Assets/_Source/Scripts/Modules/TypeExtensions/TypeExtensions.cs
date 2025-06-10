using System;
using System.Linq;

namespace RainyPlace
{
    public static class TypeExtensions
    {
        public static string GetCorrectName(this Type type)
        {
            if (type.IsGenericType == false)
                return type.Name;

            string typeName = type.Name.Split('`')[0];
            string genericArgs = string.Join(
                ", ", type.GetGenericArguments().Select(t => t.Name));

            return $"{typeName}<{genericArgs}>";
        }
    }
}
