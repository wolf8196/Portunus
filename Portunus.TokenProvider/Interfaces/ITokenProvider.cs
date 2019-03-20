using Portunus.TokenProvider.Models;

namespace Portunus.TokenProvider.Interfaces
{
    public interface ITokenProvider
    {
        string Issue(AccessToken token);

        bool TryVerify(string s, out AccessToken token);
    }
}