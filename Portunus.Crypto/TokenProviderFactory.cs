using Portunus.Crypto.Interfaces;
using Portunus.Crypto.Settings;

namespace Portunus.Crypto
{
    public class TokenProviderFactory : ITokenProviderFactory
    {
        public ITokenProvider Create(TokenProviderSettings settings)
        {
            return new TokenProvider(settings);
        }
    }
}