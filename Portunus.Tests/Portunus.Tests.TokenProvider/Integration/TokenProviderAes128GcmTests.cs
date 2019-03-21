using System.Diagnostics.CodeAnalysis;
using Portunus.Crypto.AuthenticatedEncryption;
using Portunus.Crypto.KeyGeneration;
using Portunus.Crypto.RSAEncryption;

namespace Portunus.TokenProvider.Tests.Integration
{
    [ExcludeFromCodeCoverage]
    public class TokenProviderAes128GcmTests : TokenProviderTests
    {
        protected override TokenProviderFactory CreateFactory()
        {
            return new TokenProviderFactory(
                new RSAEncryptor(),
                new PemRSAKeyParser(),
                new SymmetricKeyGenerator(),
                new Aes128Gcm());
        }
    }
}