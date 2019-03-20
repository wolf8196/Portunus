using Portunus.Crypto.MessageAuthentication;

namespace Portunus.Crypto.AuthenticatedEncryption
{
    public class Aes128CbcHmacSha256 : AesCbcHmacSha
    {
        public Aes128CbcHmacSha256()
            : base(16, 16, 16)
        {
        }

        private protected override Hmac CreateHmac()
        {
            return new HmacSha256();
        }
    }
}