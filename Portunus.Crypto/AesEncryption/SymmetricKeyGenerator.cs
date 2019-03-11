using System.Security.Cryptography;

namespace Portunus.Crypto.AesEncryption
{
    internal class SymmetricKeyGenerator
    {
        private readonly RandomNumberGenerator generator;

        public SymmetricKeyGenerator()
        {
            generator = RandomNumberGenerator.Create();
        }

        public byte[] Generate(int length)
        {
            var buffer = new byte[length];

            generator.GetBytes(buffer);

            return buffer;
        }
    }
}