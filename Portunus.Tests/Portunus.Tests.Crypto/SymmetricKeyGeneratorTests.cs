using System.Diagnostics.CodeAnalysis;
using Portunus.Crypto.KeyGeneration;
using Xunit;

namespace Portunus.Crypto.Tests
{
    [ExcludeFromCodeCoverage]
    public class SymmetricKeyGeneratorTests
    {
        [Theory]
        [InlineData(10)]
        [InlineData(0)]
        [InlineData(32)]
        [InlineData(64)]
        public void GeneratesByteArrayOfRequestedLength(int length)
        {
            // Arrange
            var target = new SymmetricKeyGenerator();

            // Act
            var actual = target.Generate(length);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(actual.Length, length);
        }
    }
}