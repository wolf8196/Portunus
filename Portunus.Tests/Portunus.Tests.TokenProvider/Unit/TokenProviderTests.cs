using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using Moq;
using Portunus.Crypto.Interfaces;
using Portunus.Crypto.Tests.TheoryData;
using Portunus.TokenProvider;
using Portunus.TokenProvider.Models;
using Portunus.TokenProvider.Settings;
using Xunit;

namespace Portunus.Crypto.Tests.Unit
{
    [ExcludeFromCodeCoverage]
    public class TokenProviderTests
    {
        private readonly Mock<IRSAEncryptor> rsaEncryptorMock;
        private readonly Mock<IRSAKeyParser> rsaKeyParserMock;
        private readonly Mock<ISymmetricKeyGenerator> keyGeneratorMock;
        private readonly Mock<IAEADEncryptor> aeadEncryptorMock;

        public TokenProviderTests()
        {
            rsaEncryptorMock = new Mock<IRSAEncryptor>();
            rsaKeyParserMock = new Mock<IRSAKeyParser>();
            keyGeneratorMock = new Mock<ISymmetricKeyGenerator>();
            aeadEncryptorMock = new Mock<IAEADEncryptor>();
        }

        [Fact]
        public void ThrowsOnIssueIfTokenIsNull()
        {
            // Arrange
            rsaKeyParserMock.Setup(x => x.TryParsePublic(It.IsAny<string>(), out It.Ref<RSAParameters>.IsAny)).Returns(true);
            rsaKeyParserMock.Setup(x => x.TryParsePrivate(It.IsAny<string>(), out It.Ref<RSAParameters>.IsAny)).Returns(true);

            var factory = new TokenProviderFactory(
                rsaEncryptorMock.Object,
                rsaKeyParserMock.Object,
                keyGeneratorMock.Object,
                aeadEncryptorMock.Object);

            var target = factory.Create(new TokenProviderSettings());

            // Act/Assert
            var ex = Assert.Throws<ArgumentNullException>("token", () => target.Issue(null));
        }

        [Theory]
        [MemberData(nameof(TokenProviderTheoryData.InvalidTokens), MemberType = typeof(TokenProviderTheoryData))]
        public void RejectsInvalidTokens(string token)
        {
            // Arrange
            rsaKeyParserMock.Setup(x => x.TryParsePublic(It.IsAny<string>(), out It.Ref<RSAParameters>.IsAny)).Returns(true);
            rsaKeyParserMock.Setup(x => x.TryParsePrivate(It.IsAny<string>(), out It.Ref<RSAParameters>.IsAny)).Returns(true);

            var factory = new TokenProviderFactory(
                rsaEncryptorMock.Object,
                rsaKeyParserMock.Object,
                keyGeneratorMock.Object,
                aeadEncryptorMock.Object);

            var target = factory.Create(new TokenProviderSettings());

            // Act
            var result = target.TryVerify(token, out AccessToken actual);

            // Assert
            Assert.False(result);
            Assert.Null(actual);
        }
    }
}