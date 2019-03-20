using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using KellermanSoftware.CompareNetObjects;
using Microsoft.AspNetCore.WebUtilities;
using Portunus.Crypto.Tests.TheoryData;
using Portunus.Tests.Shared.Extensions;
using Portunus.Tests.Shared.TestData;
using Portunus.TokenProvider.Models;
using Portunus.TokenProvider.Settings;
using Xunit;

namespace Portunus.TokenProvider.Tests.Integration
{
    [ExcludeFromCodeCoverage]
    public abstract class TokenProviderTests
    {
        [Theory]
        [MemberData(nameof(TokenProviderTheoryData.AccessTokens), MemberType = typeof(TokenProviderTheoryData))]
        public void IssuesAndVerifiesToken(AccessToken tokenObj)
        {
            // Arrange
            var factory = CreateFactory();
            var target = factory.Create(CreateSettings());
            var tokenStr = target.Issue(tokenObj);

            // Act
            var result = target.TryVerify(tokenStr, out AccessToken actual);

            // Assert
            Assert.True(result);
            Assert.True(new CompareLogic().Compare(tokenObj, actual).AreEqual);
        }

        [Theory]
        [MemberData(nameof(TokenProviderTheoryData.AccessTokens), MemberType = typeof(TokenProviderTheoryData))]
        public void RejectsDamagedTokens(AccessToken accessToken)
        {
            // Arrange
            var factory = CreateFactory();
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

        protected TokenProviderSettings CreateSettings()
        {
            return new TokenProviderSettings
            {
                PublicKey = RSAKeys.PublicKey2048,
                PrivateKey = RSAKeys.PrivateKey2048
            };
        }

        protected abstract TokenProviderFactory CreateFactory();
    }
}