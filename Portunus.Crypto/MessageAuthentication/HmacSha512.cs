using System.Security.Cryptography;

namespace Portunus.Crypto.MessageAuthentication
{
    internal class HmacSha512 : Hmac
    {
        protected override HMAC Create(byte[] key)
        {
            return new HMACSHA512(key);
        }
    }
}