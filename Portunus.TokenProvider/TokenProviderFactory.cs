using Portunus.Crypto.Interfaces;
using Portunus.TokenProvider.Interfaces;
using Portunus.TokenProvider.Settings;
using Portunus.Utils;

namespace Portunus.TokenProvider
{
    public class TokenProviderFactory : ITokenProviderFactory
    {
        private readonly IRSAEncryptor rsaEncryptor;
        private readonly IRSAKeyParser rsaKeyParser;
        private readonly ISymmetricKeyGenerator keyGenerator;
        private readonly IAEADEncryptor aeadEncryptor;

        public TokenProviderFactory(
            IRSAEncryptor rsaEncryptor,
            IRSAKeyParser rsaKeyParser,
            ISymmetricKeyGenerator keyGenerator,
            IAEADEncryptor aeadEncryptor)
        {
            this.rsaEncryptor = rsaEncryptor.ThrowIfNull(nameof(rsaEncryptor));
            this.rsaKeyParser = rsaKeyParser.ThrowIfNull(nameof(rsaKeyParser));
            this.keyGenerator = keyGenerator.ThrowIfNull(nameof(keyGenerator));
            this.aeadEncryptor = aeadEncryptor.ThrowIfNull(nameof(aeadEncryptor));
        }

        public ITokenProvider Create(TokenProviderSettings settings)
        {
            return new TokenProvider(rsaEncryptor, rsaKeyParser, keyGenerator, aeadEncryptor, settings);
        }
    }
}