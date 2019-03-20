using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using Portunus.Crypto.RSAEncryption;
using Portunus.Crypto.Tests.TheoryData;
using Xunit;

namespace Portunus.Crypto.Tests
{
    [ExcludeFromCodeCoverage]
    public class RSAEncryptorTests
    {
        [Theory]
        [MemberData(nameof(RSAEncryptorTheoryData.ValidByteMessages), MemberType = typeof(RSAEncryptorTheoryData))]
        public void EncryptsAndDecryptsMessages(byte[] message, string pubKey, string privKey)
        {
            // Arrange
            var keyParser = new PemRSAKeyParser();
            var target = new RSAEncryptor();
            keyParser.TryParsePublic(pubKey, out RSAParameters publicKey);
            keyParser.TryParsePrivate(privKey, out RSAParameters privateKey);

            // Act
            var encryptedMessage = target.Encrypt(message, publicKey);
            var decryptedMessage = target.Decrypt(encryptedMessage, privateKey);

            // Assert
            Assert.Equal(message, decryptedMessage);
        }

        [Theory]
        [MemberData(nameof(RSAEncryptorTheoryData.InvalidByteMessages), MemberType = typeof(RSAEncryptorTheoryData))]
        public void ThrowsIfMessageIsTooLongForSpecifiedKey(byte[] message, string pubKey)
        {
            // Arrange
            var keyParser = new PemRSAKeyParser();
            var target = new RSAEncryptor();
            keyParser.TryParsePublic(pubKey, out RSAParameters publicKey);

            // Act/Assert
            var ex = Assert.ThrowsAny<CryptographicException>(() => target.Encrypt(message, publicKey));
        }
    }
}