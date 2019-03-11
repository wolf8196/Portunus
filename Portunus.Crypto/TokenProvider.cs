using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Portunus.Crypto.AesEncryption;
using Portunus.Crypto.Interfaces;
using Portunus.Crypto.Models;
using Portunus.Crypto.RSAEncryption;
using Portunus.Crypto.Settings;
using Portunus.Crypto.Signing;
using Portunus.Utils;

namespace Portunus.Crypto
{
    internal class TokenProvider : ITokenProvider
    {
        private const int AesKeySize = 32;
        private const string Separator = ".";
        private const int TokenPartsCount = 3;

        private static readonly SymmetricKeyGenerator SymmetricKeyGenerator;
        private static readonly AesEncryptor AesEncryptor;
        private static readonly RSAEncryptor RsaEncryptor;
        private static readonly IDigitalSigner DigitalSigner;
        private static readonly IRSAKeyParser RSAKeyParser;

        private readonly TokenProviderSettings settings;

        private readonly RSAParameters publicKey;
        private readonly RSAParameters privateKey;

        static TokenProvider()
        {
            SymmetricKeyGenerator = new SymmetricKeyGenerator();
            AesEncryptor = new AesEncryptor();
            RsaEncryptor = new RSAEncryptor();
            DigitalSigner = new HmacSha256Signer();
            RSAKeyParser = new PemRSAKeyParser();
        }

        public TokenProvider(TokenProviderSettings settings)
        {
            this.settings = settings.ThrowIfNull(nameof(settings));

            if (!RSAKeyParser.TryParsePublic(settings.PublicKey, out publicKey))
            {
                throw new ArgumentException("Invalid RSA key", nameof(settings.PublicKey));
            }

            if (!RSAKeyParser.TryParsePrivate(settings.PrivateKey, out privateKey))
            {
                throw new ArgumentException("Invalid RSA key", nameof(settings.PrivateKey));
            }
        }

        public string Issue(AccessToken token)
        {
            token.ThrowIfNull(nameof(token));

            var aesKey = SymmetricKeyGenerator.Generate(AesKeySize);
            var hmacKey = SymmetricKeyGenerator.Generate(DigitalSigner.KeySize);

            var encryptedPayload = AesEncryptor.Encrypt(
                Encoding.UTF8.GetBytes(
                    JsonConvert.SerializeObject(token)),
                aesKey);

            var signature = DigitalSigner.Sign(encryptedPayload, hmacKey);

            var encryptedKey = RsaEncryptor.Encrypt(aesKey.Concat(hmacKey).ToArray(), publicKey);

            return string.Join(
                Separator,
                WebEncoders.Base64UrlEncode(encryptedKey),
                WebEncoders.Base64UrlEncode(encryptedPayload),
                WebEncoders.Base64UrlEncode(signature));
        }

        public bool TryVerify(string tokenStr, out AccessToken tokenObj)
        {
            tokenObj = null;

            try
            {
                if (string.IsNullOrEmpty(tokenStr))
                {
                    return false;
                }

                var tokenParts = tokenStr.Split(Separator);

                if (tokenParts.Length != TokenPartsCount)
                {
                    return false;
                }

                var encryptedKey = WebEncoders.Base64UrlDecode(tokenParts[0]);
                var encryptedPayload = WebEncoders.Base64UrlDecode(tokenParts[1]);
                var signature = WebEncoders.Base64UrlDecode(tokenParts[2]);

                var key = RsaEncryptor.Decrypt(encryptedKey, privateKey);
                var aesKey = key.AsSpan(0, AesKeySize).ToArray();
                var hmacKey = key.AsSpan(AesKeySize, key.Length - AesKeySize).ToArray();

                if (!DigitalSigner.Verify(encryptedPayload, hmacKey, signature))
                {
                    return false;
                }

                tokenObj = JsonConvert.DeserializeObject<AccessToken>(
                    Encoding.UTF8.GetString(
                        AesEncryptor.Decrypt(encryptedPayload, aesKey)));

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}