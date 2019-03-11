using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using KellermanSoftware.CompareNetObjects;
using Microsoft.AspNetCore.WebUtilities;
using Portunus.Crypto.Models;
using Portunus.Crypto.Settings;
using Portunus.Crypto.Tests.TestData;
using Portunus.Crypto.Tests.TheoryData;
using Portunus.Tests.Utils;
using Xunit;

namespace Portunus.Crypto.Tests
{
    [ExcludeFromCodeCoverage]
    public class TokenProviderTests
    {
        private readonly TokenProviderFactory factory;

        public TokenProviderTests()
        {
            factory = new TokenProviderFactory();
        }

        [Fact]
        public void IssuingThrowsIfPayloadIsNull()
        {
            // Arrange
            var target = factory.Create(CreateSettings());

            // Act/Assert
            var ex = Assert.Throws<ArgumentNullException>(() => target.Issue(null));
        }

        [Theory]
        [MemberData(nameof(TokenProviderTheoryData.InvalidTokens), MemberType = typeof(TokenProviderTheoryData))]
        public void RejectsInvalidTokens(string token)
        {
            // Arrange
            var target = factory.Create(CreateSettings());

            // Act
            var result = target.TryVerify(token, out AccessToken actual);

            // Assert
            Assert.False(result);
            Assert.Null(actual);
        }

        [Theory]
        [MemberData(nameof(TokenProviderTheoryData.AccessTokens), MemberType = typeof(TokenProviderTheoryData))]
        public void RejectsDamagedTokens(AccessToken accessToken)
        {
            // Arrange
            var target = factory.Create(CreateSettings());

            // Act
            var tokenStr = target.Issue(accessToken);
            var tokenSplitted = tokenStr.Split(".");

            for (int i = 0; i < tokenSplitted.Length; i++)
            {
                var item = tokenSplitted[i];
                var tokenSplittedCopy = tokenSplitted.Clone() as string[];

                var bits = new BitArray(WebEncoders.Base64UrlDecode(item));

                Parallel.For(
                  0,
                  bits.Count,
                  (j) =>
                  {
                      var bitsCopy = new BitArray(bits);
                      bitsCopy[j] = !bitsCopy[j];

                      tokenSplittedCopy[i] = WebEncoders.Base64UrlEncode(bitsCopy.ToByteArray());
                      var invalidToken = string.Join(".", tokenSplittedCopy);

                      var result = target.TryVerify(invalidToken, out AccessToken actual);

                      // Assert
                      Assert.False(result);
                      Assert.Null(actual);
                  });
            }
        }

        [Theory]
        [MemberData(nameof(TokenProviderTheoryData.AccessTokens), MemberType = typeof(TokenProviderTheoryData))]
        public void IssuesAndVerifiesToken(AccessToken tokenObj)
        {
            // Arrange
            var target = factory.Create(CreateSettings());

            // Act
            var tokenStr = target.Issue(tokenObj);
            var result = target.TryVerify(tokenStr, out AccessToken actual);

            // Assert
            Assert.True(result);
            Assert.True(new CompareLogic().Compare(tokenObj, actual).AreEqual);
        }

        private TokenProviderSettings CreateSettings()
        {
            return new TokenProviderSettings
            {
                PrivateKey = RSAKeys.PrivateKey2048,
                PublicKey = RSAKeys.PublicKey2048
            };
        }
    }
}