using System.Diagnostics.CodeAnalysis;
using Portunus.Crypto.AuthenticatedEncryption;
using Portunus.Crypto.Tests;

namespace Portunus.Tests.Crypto
{
    [ExcludeFromCodeCoverage]
    public class AEADAes256GcmTests : AEADTests
    {
        public AEADAes256GcmTests()
            : base(new Aes256Gcm())
        {
        }
    }
}