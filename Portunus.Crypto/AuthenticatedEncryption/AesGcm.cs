using System;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Portunus.Crypto.Interfaces;
using Portunus.Crypto.KeyGeneration;
using Portunus.Utils;

namespace Portunus.Crypto.AuthenticatedEncryption
{
    public class AesGcm : IAEADEncryptor
    {
        private readonly ISymmetricKeyGenerator keyGenerator;

        private readonly int ivSize;
        private readonly int macSize;

        public AesGcm(int keySize)
        {
            KeySize = keySize;
            ivSize = 16;
            macSize = 16;
            keyGenerator = new SymmetricKeyGenerator();
        }

        public int KeySize { get; }

        public byte[] Encrypt(byte[] plainText, byte[] key, byte[] associatedData = null)
        {
            plainText.ThrowIfNull(nameof(plainText));
            key.ThrowIfNull(nameof(key));

            if (key.Length != KeySize)
            {
                throw new ArgumentException("Invalid key size", nameof(key));
            }

            var iv = keyGenerator.Generate(ivSize);

            var cipher = new GcmBlockCipher(new AesEngine());
            var parameters = new AeadParameters(new KeyParameter(key), macSize * 8, iv, associatedData);

            cipher.Init(true, parameters);

            var result = new byte[ivSize + cipher.GetOutputSize(plainText.Length)];
            iv.CopyTo(result, 0);

            var processedLength = cipher.ProcessBytes(plainText, 0, plainText.Length, result, ivSize);
            cipher.DoFinal(result, processedLength + ivSize);

            return result;
        }

        public byte[] Decrypt(byte[] cipherText, byte[] key, byte[] associatedData = null)
        {
            cipherText.ThrowIfNull(nameof(cipherText));
            key.ThrowIfNull(nameof(key));

            if (key.Length != KeySize)
            {
                throw new ArgumentException("Invalid key size", nameof(key));
            }

            try
            {
                if (cipherText.Length < macSize)
                {
                    return null;
                }

                var iv = cipherText.AsSpan(0, ivSize).ToArray();

                var cipher = new GcmBlockCipher(new AesEngine());
                var parameters = new AeadParameters(new KeyParameter(key), macSize * 8, iv, associatedData);

                cipher.Init(false, parameters);

                var plainText = new byte[cipher.GetOutputSize(cipherText.Length - ivSize)];

                var processedLength = cipher.ProcessBytes(cipherText, ivSize, cipherText.Length - ivSize, plainText, 0);
                cipher.DoFinal(plainText, processedLength);

                return plainText;
            }
            catch
            {
                return null;
            }
        }
    }
}