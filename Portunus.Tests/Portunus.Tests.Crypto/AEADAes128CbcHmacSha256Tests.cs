using System.Diagnostics.CodeAnalysis;
using Portunus.Crypto.AuthenticatedEncryption;

namespace Portunus.Crypto.Tests
{
    [ExcludeFromCodeCoverage]
    public class AEADAes128CbcHmacSha256Tests : AEADTests
    {
        public AEADAes128CbcHmacSha256Tests()
            : base(new Aes128CbcHmacSha256())
        {
        }
    }
}