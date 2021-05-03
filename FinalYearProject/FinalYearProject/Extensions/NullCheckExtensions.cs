using System;

namespace FinalYearProject.Extensions
{
    public static class NullCheckExtensions
    {
        public static void ThrowIfNull<T>(this T target, string paramName, string message = null)
        {
            if (target is null)
            {
                if (message is null)
                {
                    throw new ArgumentNullException(paramName);
                }
                else
                {
                    throw new ArgumentNullException(paramName, message);
                }
            }
        }

        public static void ThrowIfNullOrEmpty(this string target, string paramName, string message = null)
        {
            if (string.IsNullOrEmpty(target))
            {
                if (message is null)
                {
                    throw new ArgumentNullException(paramName);
                }
                else
                {
                    throw new ArgumentNullException(paramName, message);
                }
            }
        }
    }
}