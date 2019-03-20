using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Portunus.Crypto.Interfaces;
using Portunus.TokenProvider.Interfaces;
using Portunus.TokenProvider.Models;
using Portunus.TokenProvider.Settings;
using Portunus.Utils;

namespace Portunus.TokenProvider
{
    internal class TokenProvider : ITokenProvider
    {
        private const string Separator = ".";
        private const int TokenPartsCount = 2;

        private readonly IRSAEncryptor rsaEncryptor;
        private readonly IRSAKeyParser rsaKeyParser;

        private readonly ISymmetricKeyGenerator keyGenerator;
        private readonly IAEADEncryptor aeadEncryptor;

        private readonly TokenProviderSettings settings;

        private readonly RSAParameters publicKey;
        private readonly RSAParameters privateKey;

        public TokenProvider(
            IRSAEncryptor rsaEncryptor,
            IRSAKeyParser rsaKeyParser,
            ISymmetricKeyGenerator keyGenerator,
            IAEADEncryptor aeadEncryptor,
            TokenProviderSettings settings)
        {
            this.rsaEncryptor = rsaEncryptor;
            this.rsaKeyParser = rsaKeyParser;
            this.keyGenerator = keyGenerator;
            this.aeadEncryptor = aeadEncryptor;

            this.settings = settings.ThrowIfNull(nameof(settings));

            if (!rsaKeyParser.TryParsePublic(settings.PublicKey, out publicKey))
            {
                throw new ArgumentException("Invalid RSA key", nameof(settings.PublicKey));
            }

            if (!rsaKeyParser.TryParsePrivate(settings.PrivateKey, out privateKey))
            {
                throw new ArgumentException("Invalid RSA key", nameof(settings.PrivateKey));
            }
        }

        public string Issue(AccessToken token)
        {
            token.ThrowIfNull(nameof(token));

            var key = keyGenerator.Generate(aeadEncryptor.KeySize);

            var encryptedPayload = aeadEncryptor.Encrypt(
                Encoding.UTF8.GetBytes(
                    JsonConvert.SerializeObject(token)),
                key);

            var encryptedKey = rsaEncryptor.Encrypt(key, publicKey);

            return string.Join(
                Separator,
                WebEncoders.Base64UrlEncode(encryptedKey),
                WebEncoders.Base64UrlEncode(encryptedPayload));
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

                var key = rsaEncryptor.Decrypt(encryptedKey, privateKey);

                var plainText = aeadEncryptor.Decrypt(encryptedPayload, key);

                if (plainText == null)
                {
                    return false;
                }

                tokenObj = JsonConvert.DeserializeObject<AccessToken>(
                    Encoding.UTF8.GetString(plainText));

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}