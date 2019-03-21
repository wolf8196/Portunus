namespace Portunus.Crypto.AuthenticatedEncryption
{
    public class Aes256Gcm : AesGcm
    {
        public Aes256Gcm()
            : base(32)
        {
        }
    }
}