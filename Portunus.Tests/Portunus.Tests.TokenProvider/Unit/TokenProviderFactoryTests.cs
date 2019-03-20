using System;
using System.Diagnostics.CodeAnalysis;
using Moq;
using Portunus.Crypto.Interfaces;
using Portunus.TokenProvider.Tests.TheoryData;
using Xunit;

namespace Portunus.TokenProvider.Tests.Unit
{
    [ExcludeFromCodeCoverage]
    public class TokenProviderFactoryTests
    {
        [Theory]
        [MemberData(nameof(TokenProviderFactoryTheoryData.InvalidDependencies), MemberType = typeof(TokenProviderFactoryTheoryData))]
        public void ThrowsIfDependenciesAreNull(
            IRSAEncryptor rsaEncryptor,
            IRSAKeyParser rsaKeyParser,
            ISymmetricKeyGenerator keyGenerator,
            IAEADEncryptor aeadEncryptor,
            string paramName)
        {
            // Arrange/Act/Assert
            Assert.Throws<ArgumentNullException>(
                paramName,
                () => new TokenProviderFactory(rsaEncryptor, rsaKeyParser, keyGenerator, aeadEncryptor));
        }

        [Fact]
        public void CreatesTokenProviderFactory()
        {
            // Arrange/Act/Assert
            new TokenProviderFactory(
                new Mock<IRSAEncryptor>().Object,
                new Mock<IRSAKeyParser>().Object,
                new Mock<ISymmetricKeyGenerator>().Object,
                new Mock<IAEADEncryptor>().Object);
        }
    }
}