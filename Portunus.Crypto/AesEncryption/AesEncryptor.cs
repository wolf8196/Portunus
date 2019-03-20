using System;
using System.IO;
using System.Security.Cryptography;

namespace Portunus.Crypto.AesEncryption
{
    internal class AesEncryptor
    {
        public const int BlockSize = 16;

        public byte[] Encrypt(byte[] plainText, byte[] key)
        {
            using (var aesAlg = Create())
            {
                using (var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV))
                using (var resultStream = new MemoryStream())
                {
                    resultStream.Write(aesAlg.IV);
                    resultStream.Flush();

                    using (var cryptoStream = new CryptoStream(resultStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainText);
                        cryptoStream.FlushFinalBlock();
                        return resultStream.ToArray();
                    }
                }
            }
        }

        public byte[] Decrypt(byte[] cipherText, byte[] key)
        {
            using (var aesAlg = Create())
            {
                var iv = cipherText.AsSpan(0, BlockSize);
                var realCipherText = cipherText.AsSpan(BlockSize, cipherText.Length - BlockSize);

                using (var decryptor = aesAlg.CreateDecryptor(key, iv.ToArray()))
                using (var resultStream = new MemoryStream())
                using (var cryptoStream = new CryptoStream(resultStream, decryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(realCipherText);
                    cryptoStream.FlushFinalBlock();
                    return resultStream.ToArray();
                }
            }
        }

        private Aes Create()
        {
            var aesAlg = Aes.Create();
            aesAlg.Padding = PaddingMode.PKCS7;
            aesAlg.Mode = CipherMode.CBC;
            return aesAlg;
        }
    }
}