using System.Diagnostics.CodeAnalysis;
using Portunus.Crypto.AuthenticatedEncryption;
using Portunus.Crypto.KeyGeneration;
using Portunus.Crypto.RSAEncryption;

namespace Portunus.TokenProvider.Tests.Integration
{
    [ExcludeFromCodeCoverage]
    public class TokenProviderAes256CbcHmacSha512Tests : TokenProviderTests
    {
        protected override TokenProviderFactory CreateFactory()
        {
            return new TokenProviderFactory(
                new RSAEncryptor(),
                new PemRSAKeyParser(),
                new SymmetricKeyGenerator(),
                new Aes256CbcHmacSha512());
        }
    }
}