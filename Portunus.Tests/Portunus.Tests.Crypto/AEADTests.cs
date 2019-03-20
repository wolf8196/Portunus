using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Portunus.Crypto.Interfaces;
using Portunus.Crypto.Tests.TheoryData;
using Portunus.Tests.Shared.Extensions;
using Xunit;

namespace Portunus.Crypto.Tests
{
    [ExcludeFromCodeCoverage]
    public abstract class AEADTests
    {
        protected AEADTests(IAEADEncryptor encryptor)
        {
            Target = encryptor;
        }

        protected IAEADEncryptor Target { get; }

        [Theory]
        [MemberData(nameof(AEADTheoryData.InvalidEncryptParameters), MemberType = typeof(AEADTheoryData))]
        public void ThrowsOnEncryptIfParametersAreInvalid(byte[] message, byte[] key, string paramName)
        {
            // Arrange/Act/Assert
            var ex = Assert.Throws<ArgumentNullException>(paramName, () => Target.Encrypt(message, key));
        }

        [Theory]
        [MemberData(nameof(AEADTheoryData.InvalidDecryptParameters), MemberType = typeof(AEADTheoryData))]
        public void ThrowsOnDecryptIfParametersAreInvalid(byte[] message, byte[] key, string paramName)
        {
            // Arrange/Act/Assert
            var ex = Assert.Throws<ArgumentNullException>(paramName, () => Target.Decrypt(message, key));
        }

        [Theory]
        [MemberData(nameof(AEADTheoryData.ByteMessages), MemberType = typeof(AEADTheoryData))]
        public void RejectsDamagedMessages(byte[] message, byte[] associatedData)
        {
            // Arrange
            var key = new byte[Target.KeySize];
            RandomNumberGenerator.Create().GetBytes(key);

            var ecrypted = Target.Encrypt(message, key);
            var associatedDataLength = associatedData?.Length ?? 0;

            var toTest = new byte[associatedDataLength + ecrypted.Length];
            associatedData?.CopyTo(toTest, 0);
            ecrypted.CopyTo(toTest, associatedDataLength);

            var bits = new BitArray(toTest);

            Parallel.For(
              0,
              bits.Count,
              (j) =>
              {
                  var bitsCopy = new BitArray(bits);
                  bitsCopy[j] = !bitsCopy[j];

                  var fakedText = bitsCopy.ToByteArray();
                  var fakedAssociatedData = associatedDataLength != 0 ? fakedText.AsSpan(0, associatedDataLength).ToArray() : null;
                  var fakedEncryptedText = fakedText.AsSpan(associatedDataLength, fakedText.Length - associatedDataLength).ToArray();

                  // Act
                  var result = Target.Decrypt(fakedEncryptedText, key, fakedAssociatedData);

                  // Assert
                  Assert.Null(result);
              });
        }

        [Theory]
        [MemberData(nameof(AEADTheoryData.ByteMessages), MemberType = typeof(AEADTheoryData))]
        public void EncryptsAndDecryptsMessages(byte[] message, byte[] associatedData)
        {
            // Arrange
            var key = new byte[Target.KeySize];
            RandomNumberGenerator.Create().GetBytes(key);

            // Act
            var ecrypted = Target.Encrypt(message, key, associatedData);
            var actual = Target.Decrypt(ecrypted, key, associatedData);

            // Assert
            Assert.Equal(message, actual);
        }

        [Fact]
        public void RejectsTooShortMessages()
        {
            // Arrange
            var key = new byte[Target.KeySize];
            RandomNumberGenerator.Create().GetBytes(key);

            // Act
            var actual = Target.Decrypt(Encoding.UTF8.GetBytes("test"), key);

            // Assert
            Assert.Null(actual);
        }

        [Fact]
        public void RejectsInvalidEncryptionKey()
        {
            // Arrange
            var key = new byte[Target.KeySize];
            RandomNumberGenerator.Create().GetBytes(key);
            var encrypted = Target.Encrypt(Encoding.UTF8.GetBytes("test"), key);
            key[key.Length - 1] = 0;

            // Act
            var actual = Target.Decrypt(encrypted, key);

            // Assert
            Assert.Null(actual);
        }
    }
}