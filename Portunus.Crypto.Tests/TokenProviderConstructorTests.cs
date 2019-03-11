using System;
using System.Diagnostics.CodeAnalysis;
using Portunus.Crypto.Settings;
using Portunus.Crypto.Tests.TestData;
using Portunus.Crypto.Tests.TheoryData;
using Xunit;

namespace Portunus.Crypto.Tests
{
    [ExcludeFromCodeCoverage]
    public class TokenProviderConstructorTests
    {
        private readonly TokenProviderFactory factory;

        public TokenProviderConstructorTests()
        {
            factory = new TokenProviderFactory();
        }

        [Fact]
        public void ThrowsIfSettingsObjectIsNull()
        {
            // Arrange
            TokenProviderSettings settings = null;

            // Act/Assert
            var ex = Assert.Throws<ArgumentNullException>(() => factory.Create(settings));
            Assert.Equal("settings", ex.ParamName);
        }

        [Theory]
        [MemberData(nameof(TokenProviderConstructorTheoryData.InvalidSettings), MemberType = typeof(TokenProviderConstructorTheoryData))]
        public void ThrowsIfSettingsValuesAreInvalid(
            string publicKey,
            string privateKey,
            string expectedParamName)
        {
            // Arrange
            TokenProviderSettings settings = new TokenProviderSettings
            {
                PublicKey = publicKey,
                PrivateKey = privateKey
            };

            // Act/Assert
            var ex = Assert.Throws<ArgumentException>(() => factory.Create(settings));
            Assert.Equal(expectedParamName, ex.ParamName);
        }

        [Fact]
        public void CreatesTokenProvider()
        {
            // Arrange
            TokenProviderSettings settings = new TokenProviderSettings
            {
                PublicKey = RSAKeys.PublicKey512,
                PrivateKey = RSAKeys.PrivateKey512
            };

            // Act/Assert
            var actual = factory.Create(settings);
        }
    }
}