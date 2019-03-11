using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Portunus.Tests.Utils
{
    [ExcludeFromCodeCoverage]
    public static class BitArrayExtensions
    {
        public static byte[] ToByteArray(this BitArray bits)
        {
            byte[] bytes = new byte[bits.Length / 8];

            bits.CopyTo(bytes, 0);

            return bytes;
        }
    }
}