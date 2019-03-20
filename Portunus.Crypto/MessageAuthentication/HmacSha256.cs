using System.Security.Cryptography;

namespace Portunus.Crypto.MessageAuthentication
{
    internal class HmacSha256 : Hmac
    {
        protected override HMAC Create(byte[] key)
        {
            return new HMACSHA256(key);
        }
    }
}