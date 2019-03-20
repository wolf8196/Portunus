using System.Security.Cryptography;

namespace Portunus.Crypto.Interfaces
{
    public interface IRSAEncryptor
    {
        byte[] Decrypt(byte[] cipherText, RSAParameters privateKey);

        byte[] Encrypt(byte[] plainText, RSAParameters publicKey);
    }
}