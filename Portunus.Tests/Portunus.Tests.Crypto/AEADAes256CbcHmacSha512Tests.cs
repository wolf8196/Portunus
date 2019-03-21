using System.Diagnostics.CodeAnalysis;
using Portunus.Crypto.AuthenticatedEncryption;

namespace Portunus.Crypto.Tests
{
    [ExcludeFromCodeCoverage]
    public class AEADAes256CbcHmacSha512Tests : AEADTests
    {
        public AEADAes256CbcHmacSha512Tests()
            : base(new Aes256CbcHmacSha512())
        {
        }
    }
}