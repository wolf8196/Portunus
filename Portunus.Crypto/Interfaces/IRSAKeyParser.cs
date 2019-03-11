using System.Security.Cryptography;

namespace Portunus.Crypto.Interfaces
{
    internal interface IRSAKeyParser
    {
        bool TryParsePrivate(string s, out RSAParameters key);

        bool TryParsePublic(string s, out RSAParameters key);
    }
}