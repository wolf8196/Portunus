using Portunus.Crypto.MessageAuthentication;

namespace Portunus.Crypto.AuthenticatedEncryption
{
    public class Aes256CbcHmacSha512 : AesCbcHmacSha
    {
        public Aes256CbcHmacSha512()
            : base(32, 32, 32)
        {
        }

        private protected override Hmac CreateHmac()
        {
            return new HmacSha512();
        }
    }
}