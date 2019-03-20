using System;
using System.Diagnostics.CodeAnalysis;
using Portunus.Crypto.AuthenticatedEncryption;
using Xunit;

namespace Portunus.Crypto.Tests
{
    [ExcludeFromCodeCoverage]
    public class AEADAes128CbcHmacSha256Tests : AEADTests
    {
        public AEADAes128CbcHmacSha256Tests()
            : base(new Aes128CbcHmacSha256())
        {
        }

        [Theory]
        [InlineData(0)]
        [InlineData(10)]
        [InlineData(15)]
        [InlineData(31)]
        [InlineData(64)]
        public void ThrowsOnEncryptIfKeySizeIsInvalid(int length)
        {
            // Arrange
            var key = new byte[length];

            // Act/Assert
            var ex = Assert.Throws<ArgumentException>("key", () => Target.Encrypt(new byte[0], key));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(10)]
        [InlineData(15)]
        [InlineData(31)]
        [InlineData(64)]
        public void ThrowsOnDecryptIfKeySizeIsInvalid(int length)
        {
            // Arrange
            var key = new byte[length];

            // Act/Assert
            var ex = Assert.Throws<ArgumentException>("key", () => Target.Decrypt(new byte[0], key));
        }
    }
}