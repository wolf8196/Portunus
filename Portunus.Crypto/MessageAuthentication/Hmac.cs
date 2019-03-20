using System.Security.Cryptography;

namespace Portunus.Crypto.MessageAuthentication
{
    internal abstract class Hmac
    {
        public byte[] ComputeHash(byte[] message, byte[] key)
        {
            using (var hasher = Create(key))
            {
                return hasher.ComputeHash(message);
            }
        }

        protected abstract HMAC Create(byte[] key);
    }
}