using System.Diagnostics.CodeAnalysis;
using Moq;
using Portunus.Crypto.Interfaces;
using Xunit;

namespace Portunus.TokenProvider.Tests.TheoryData
{
    [ExcludeFromCodeCoverage]
    internal static class TokenProviderFactoryTheoryData
    {
        public static TheoryData<IRSAEncryptor, IRSAKeyParser, ISymmetricKeyGenerator, IAEADEncryptor, string> InvalidDependencies
        {
            get
            {
                var data = new TheoryData<IRSAEncryptor, IRSAKeyParser, ISymmetricKeyGenerator, IAEADEncryptor, string>
                {
                    { null, null, null, null, "rsaEncryptor" },
                    { new Mock<IRSAEncryptor>().Object, null, null, null, "rsaKeyParser" },
                    { new Mock<IRSAEncryptor>().Object, new Mock<IRSAKeyParser>().Object, null, null, "keyGenerator" },
                    { new Mock<IRSAEncryptor>().Object, new Mock<IRSAKeyParser>().Object, new Mock<ISymmetricKeyGenerator>().Object, null, "aeadEncryptor" }
                };

                return data;
            }
        }
    }
}