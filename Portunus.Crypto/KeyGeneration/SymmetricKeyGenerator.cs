using System.Security.Cryptography;
using Portunus.Crypto.Interfaces;

namespace Portunus.Crypto.KeyGeneration
{
    public class SymmetricKeyGenerator : ISymmetricKeyGenerator
    {
        public byte[] Generate(int length)
        {
            using (var generator = RandomNumberGenerator.Create())
            {
                var buffer = new byte[length];

                generator.GetBytes(buffer);

                return buffer;
            }
        }
    }
}