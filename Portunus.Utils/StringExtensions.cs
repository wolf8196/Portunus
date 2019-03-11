using System;

namespace Portunus.Utils
{
    public static class StringExtensions
    {
        public static string ThrowIfNullOrEmpty(this string str, string paramName)
        {
            if (str == null)
            {
                throw new ArgumentNullException(paramName);
            }

            if (str.Trim().Length == 0)
            {
                throw new ArgumentException("Argument is empty", paramName);
            }

            return str;
        }
    }
}