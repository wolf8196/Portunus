using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using Portunus.Crypto.RSAEncryption;
using Portunus.Crypto.Tests.TheoryData;
using Xunit;

namespace Portunus.Crypto.Tests
{
    [ExcludeFromCodeCoverage]
    public class PemRSAKeyParserTests
    {
        [Theory]
        [MemberData(nameof(PemRSAKeyParserTheoryData.InvalidPrivateKeys), MemberType = typeof(PemRSAKeyParserTheoryData))]
        public void FailsToParseInvalidPrivateKey(string key)
        {
            // Arrange
            var target = new PemRSAKeyParser();

            // Act
            var actual = target.TryParsePrivate(key, out RSAParameters parameters);

            // Assert
            Assert.False(actual);
            Assert.Equal(default, parameters);
        }

        [Theory]
        [MemberData(nameof(PemRSAKeyParserTheoryData.ValidPrivateKeys), MemberType = typeof(PemRSAKeyParserTheoryData))]
        public void ParsesPrivateKey(string key)
        {
            // Arrange
            var target = new PemRSAKeyParser();

            // Act
            var actual = target.TryParsePrivate(key, out RSAParameters parameters);

            // Assert
            Assert.True(actual);
            Assert.NotEqual(default, parameters);
        }

        [Theory]
        [MemberData(nameof(PemRSAKeyParserTheoryData.InvalidPublicKeys), MemberType = typeof(PemRSAKeyParserTheoryData))]
        public void FailsToParseInvalidPublicKey(string key)
        {
            // Arrange
            var target = new PemRSAKeyParser();

            // Act
            var actual = target.TryParsePublic(key, out RSAParameters parameters);

            // Assert
            Assert.False(actual);
            Assert.Equal(default, parameters);
        }

        [Theory]
        [MemberData(nameof(PemRSAKeyParserTheoryData.ValidPublicKeys), MemberType = typeof(PemRSAKeyParserTheoryData))]
        public void ParsesPublicKey(string key)
        {
            // Arrange
            var target = new PemRSAKeyParser();

            // Act
            var actual = target.TryParsePublic(key, out RSAParameters parameters);

            // Assert
            Assert.True(actual);
            Assert.NotEqual(default, parameters);
        }
    }
}