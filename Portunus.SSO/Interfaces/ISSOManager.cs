using System.Dynamic;

namespace Portunus.SSO.Interfaces
{
    public interface ISSOManager
    {
        string IssueToken(string app, ExpandoObject payload);

        bool TryVerifyToken(string app, string token, out ExpandoObject payload);
    }
}