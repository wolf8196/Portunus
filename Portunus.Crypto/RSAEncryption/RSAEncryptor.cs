using System.Security.Cryptography;

namespace Portunus.Crypto.RSAEncryption
{
    internal class RSAEncryptor
    {
        public byte[] Encrypt(byte[] plainText, RSAParameters publicKey)
        {
            using (var rsa = RSA.Create(publicKey))
            {
                return rsa.Encrypt(plainText, RSAEncryptionPadding.OaepSHA256);
            }
        }

        public byte[] Decrypt(byte[] cipherText, RSAParameters privateKey)
        {
            using (var rsa = RSA.Create(privateKey))
            {
                return rsa.Decrypt(cipherText, RSAEncryptionPadding.OaepSHA256);
            }
        }
    }
}