using System.Linq;

namespace RainyPlace
{
    public static class StringExtensions
    {
        public static string Remove(this string source, string substring)
        {
            return source.Replace(substring, string.Empty);
        }

        public static string Reverse(this string source)
        {
            return new string(source.Reverse<char>().ToArray());
        }
    }
}
