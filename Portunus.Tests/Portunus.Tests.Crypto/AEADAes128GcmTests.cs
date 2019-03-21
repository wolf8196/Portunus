using System.Diagnostics.CodeAnalysis;
using Portunus.Crypto.AuthenticatedEncryption;
using Portunus.Crypto.Tests;

namespace Portunus.Tests.Crypto
{
    [ExcludeFromCodeCoverage]
    public class AEADAes128GcmTests : AEADTests
    {
        public AEADAes128GcmTests()
            : base(new Aes128Gcm())
        {
        }
    }
}