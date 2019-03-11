using System.Diagnostics.CodeAnalysis;
using Portunus.Crypto.Settings;
using Portunus.Crypto.Tests.TestData;
using Xunit;

namespace Portunus.Crypto.Tests.TheoryData
{
    [ExcludeFromCodeCoverage]
    internal static class TokenProviderConstructorTheoryData
    {
        public static TheoryData<string, string, string> InvalidSettings
        {
            get
            {
                var publicKeyParamName = nameof(TokenProviderSettings.PublicKey);
                var privateKeyParamName = nameof(TokenProviderSettings.PrivateKey);

                var data = new TheoryData<string, string, string>
                {
                    { null, null, publicKeyParamName },
                    { string.Empty, null, publicKeyParamName },
                    { "test", null, publicKeyParamName },
                    { RSAKeys.PublicKey512, null, privateKeyParamName },
                    { RSAKeys.PublicKey512, string.Empty, privateKeyParamName },
                    { RSAKeys.PublicKey512, "test", privateKeyParamName }
                };

                return data;
            }
        }
    }
}