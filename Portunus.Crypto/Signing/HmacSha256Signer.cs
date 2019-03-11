using System.Security.Cryptography;
using Portunus.Crypto.Interfaces;

namespace Portunus.Crypto.Signing
{
    internal class HmacSha256Signer : IDigitalSigner
    {
        public int KeySize => 32;

        public int HashSize => 32;

        public byte[] Sign(byte[] message, byte[] key)
        {
            using (var hasher = new HMACSHA256(key))
            {
                var hash = hasher.ComputeHash(message);

                return hash;
            }
        }

        public bool Verify(byte[] message, byte[] key, byte[] signature)
        {
            if (signature.Length != HashSize)
            {
                return false;
            }

            using (var hasher = new HMACSHA256(key))
            {
                var hash = hasher.ComputeHash(message);

                for (int i = 0; i < hash.Length; i++)
                {
                    if (hash[i] != signature[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}