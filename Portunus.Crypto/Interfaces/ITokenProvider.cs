using Portunus.Crypto.Models;

namespace Portunus.Crypto.Interfaces
{
    public interface ITokenProvider
    {
        string Issue(AccessToken token);

        bool TryVerify(string s, out AccessToken token);
    }
}