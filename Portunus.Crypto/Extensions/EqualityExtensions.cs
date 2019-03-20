using System;

namespace Portunus.Crypto.Extensions
{
    public static class EqualityExtensions
    {
        public static bool IsEqualTo(this Span<byte> span, byte[] array)
        {
            if (span.Length != array.Length)
            {
                return false;
            }

            for (int i = 0; i < span.Length; i++)
            {
                if (span[i] != array[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}