using System;
using System.Linq;
using Portunus.Crypto.AesEncryption;
using Portunus.Crypto.Extensions;
using Portunus.Crypto.Interfaces;
using Portunus.Crypto.MessageAuthentication;
using Portunus.Utils;

namespace Portunus.Crypto.AuthenticatedEncryption
{
    public abstract class AesCbcHmacSha : IAEADEncryptor
    {
        private readonly AesEncryptor aesEncryptor;
        private readonly Hmac hasher;

        private readonly int encryptionKeySize;
        private readonly int macKeySize;
        private readonly int macSize;

        protected AesCbcHmacSha(
            int macKeySize,
            int encryptionKeySize,
            int macSize)
        {
            aesEncryptor = new AesEncryptor();
            hasher = CreateHmac();

            this.macKeySize = macKeySize;
            this.encryptionKeySize = encryptionKeySize;
            this.macSize = macSize;
        }

        public int KeySize { get => macKeySize + encryptionKeySize; }

        public byte[] Encrypt(byte[] plainText, byte[] key, byte[] associatedData = null)
        {
            plainText.ThrowIfNull(nameof(plainText));
            key.ThrowIfNull(nameof(key));

            if (key.Length != KeySize)
            {
                throw new ArgumentException("Invalid key size", nameof(key));
            }

            var (macKey, encryptionKey) = SplitKeys(key);

            var encryptedText = aesEncryptor.Encrypt(plainText, encryptionKey);

            var authenticationTag = CreateAuthenticationTag(encryptedText, macKey, associatedData);

            return CreateResult(encryptedText, authenticationTag);
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

                var (macKey, encryptionKey) = SplitKeys(key);

                var authenticationTag = cipherText.AsSpan(cipherText.Length - macSize, macSize);
                var encryptedText = cipherText.AsSpan(0, cipherText.Length - macSize).ToArray();

                if (!authenticationTag.IsEqualTo(CreateAuthenticationTag(encryptedText, macKey, associatedData)))
                {
                    return null;
                }

                return aesEncryptor.Decrypt(encryptedText, encryptionKey);
            }
            catch
            {
                return null;
            }
        }

        private protected abstract Hmac CreateHmac();

        private byte[] CreateAuthenticationTag(byte[] encryptedText, byte[] key, byte[] associatedData)
        {
            var associatedDataLength = associatedData?.Length ?? 0;
            var associatedDataLengthBytes = BitConverter.GetBytes(Convert.ToUInt64(associatedDataLength * 8));
            Array.Reverse(associatedDataLengthBytes);

            var toMac = new byte[associatedDataLength + encryptedText.Length + associatedDataLengthBytes.Length];

            if (associatedData != null)
            {
                associatedData.CopyTo(toMac, 0);
            }

            encryptedText.CopyTo(toMac, associatedDataLength);
            associatedDataLengthBytes.CopyTo(toMac, associatedDataLength + encryptedText.Length);

            return hasher.ComputeHash(toMac, key).AsSpan(0, macSize).ToArray();
        }

        private byte[] CreateResult(byte[] cipherText, byte[] authenticationTag)
        {
            var result = new byte[cipherText.Length + macSize];

            cipherText.CopyTo(result, 0);
            authenticationTag.CopyTo(result, cipherText.Length);

            return result;
        }

        private (byte[] macKey, byte[] encryptionKey) SplitKeys(byte[] key)
        {
            return (key.AsSpan(0, macKeySize).ToArray(), key.AsSpan(macKeySize, encryptionKeySize).ToArray());
        }
    }
}