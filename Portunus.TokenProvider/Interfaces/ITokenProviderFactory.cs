using Portunus.TokenProvider.Settings;

namespace Portunus.TokenProvider.Interfaces
{
    public interface ITokenProviderFactory
    {
        ITokenProvider Create(TokenProviderSettings settings);
    }
}