using System.Security.Cryptography;
using Portunus.Crypto.Interfaces;

namespace Portunus.Crypto.RSAEncryption
{
    public class RSAEncryptor : IRSAEncryptor
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