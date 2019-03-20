using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using Moq;
using Portunus.Crypto.Interfaces;
using Portunus.TokenProvider;
using Portunus.TokenProvider.Settings;
using Xunit;

namespace Portunus.Crypto.Tests.Unit
{
    [ExcludeFromCodeCoverage]
    public class TokenProviderConstructorTests
    {
        private readonly Mock<IRSAEncryptor> rsaEncryptorMock;
        private readonly Mock<IRSAKeyParser> rsaKeyParserMock;
        private readonly Mock<ISymmetricKeyGenerator> keyGeneratorMock;
        private readonly Mock<IAEADEncryptor> aeadEncryptorMock;

        public TokenProviderConstructorTests()
        {
            rsaEncryptorMock = new Mock<IRSAEncryptor>();
            rsaKeyParserMock = new Mock<IRSAKeyParser>();
            keyGeneratorMock = new Mock<ISymmetricKeyGenerator>();
            aeadEncryptorMock = new Mock<IAEADEncryptor>();
        }

        [Fact]
        public void ThrowsIfSettingsObjectIsNull()
        {
            // Arrange
            TokenProviderSettings settings = null;
            var target = new TokenProviderFactory(
                rsaEncryptorMock.Object,
                rsaKeyParserMock.Object,
                keyGeneratorMock.Object,
                aeadEncryptorMock.Object);

            // Act/Assert
            var ex = Assert.Throws<ArgumentNullException>("settings", () => target.Create(settings));
        }

        [Fact]
        public void ThrowsIfPublicKeyIsInvalid()
        {
            // Arrange
            TokenProviderSettings settings = new TokenProviderSettings();
            rsaKeyParserMock.Setup(x => x.TryParsePublic(It.IsAny<string>(), out It.Ref<RSAParameters>.IsAny)).Returns(false);
            var target = new TokenProviderFactory(
                rsaEncryptorMock.Object,
                rsaKeyParserMock.Object,
                keyGeneratorMock.Object,
                aeadEncryptorMock.Object);

            // Act/Assert
            var ex = Assert.Throws<ArgumentException>("PublicKey", () => target.Create(settings));
        }

        [Fact]
        public void ThrowsIfPrivateKeyIsInvalid()
        {
            // Arrange
            TokenProviderSettings settings = new TokenProviderSettings();
            rsaKeyParserMock.Setup(x => x.TryParsePublic(It.IsAny<string>(), out It.Ref<RSAParameters>.IsAny)).Returns(true);
            rsaKeyParserMock.Setup(x => x.TryParsePrivate(It.IsAny<string>(), out It.Ref<RSAParameters>.IsAny)).Returns(false);
            var target = new TokenProviderFactory(
                rsaEncryptorMock.Object,
                rsaKeyParserMock.Object,
                keyGeneratorMock.Object,
                aeadEncryptorMock.Object);

            // Act/Assert
            var ex = Assert.Throws<ArgumentException>("PrivateKey", () => target.Create(settings));
        }

        [Fact]
        public void CreatesTokenProvider()
        {
            // Arrange
            TokenProviderSettings settings = new TokenProviderSettings();
            rsaKeyParserMock.Setup(x => x.TryParsePublic(It.IsAny<string>(), out It.Ref<RSAParameters>.IsAny)).Returns(true);
            rsaKeyParserMock.Setup(x => x.TryParsePrivate(It.IsAny<string>(), out It.Ref<RSAParameters>.IsAny)).Returns(true);
            var target = new TokenProviderFactory(
                rsaEncryptorMock.Object,
                rsaKeyParserMock.Object,
                keyGeneratorMock.Object,
                aeadEncryptorMock.Object);

            // Act/Assert
            target.Create(settings);
        }
    }
}