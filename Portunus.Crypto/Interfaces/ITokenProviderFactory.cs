using Portunus.Crypto.Settings;

namespace Portunus.Crypto.Interfaces
{
    public interface ITokenProviderFactory
    {
        ITokenProvider Create(TokenProviderSettings settings);
    }
}