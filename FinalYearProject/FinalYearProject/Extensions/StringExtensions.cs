using System.Linq;

namespace FinalYearProject.Extensions
{
    public static class StringExtensions
    {
        public static string AddSpaceBeforeCapitalLetters(this string str)
        {
            if (str is null)
            {
                return string.Empty;
            }

            return string.Concat(str.Select(x => char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');
        }

        public static string RemoveSpaces(this string str)
        {
            if (str is null)
            {
                return string.Empty;
            }

            return str.Replace(" ", string.Empty);
        }
    }
}
