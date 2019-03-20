using System.Diagnostics.CodeAnalysis;
using Bogus;
using Portunus.Tests.Shared.TestData;
using Xunit;

namespace Portunus.Crypto.Tests.TheoryData
{
    [ExcludeFromCodeCoverage]
    public static class RSAEncryptorTheoryData
    {
        private static readonly Faker Faker;

        static RSAEncryptorTheoryData()
        {
            Faker = new Faker();
        }

        public static TheoryData<byte[], string, string> ValidByteMessages
        {
            get
            {
                var data = new TheoryData<byte[], string, string>
                {
                    { Faker.Random.Bytes(62), RSAKeys.PublicKey1024, RSAKeys.PrivateKey1024 },
                    { Faker.Random.Bytes(35), RSAKeys.PublicKey1024, RSAKeys.PrivateKey1024 },
                    { Faker.Random.Bytes(128), RSAKeys.PublicKey2048, RSAKeys.PrivateKey2048 },
                    { Faker.Random.Bytes(190), RSAKeys.PublicKey2048, RSAKeys.PrivateKey2048 },
                    { Faker.Random.Bytes(256), RSAKeys.PublicKey4096, RSAKeys.PrivateKey4096 },
                    { Faker.Random.Bytes(446), RSAKeys.PublicKey4096, RSAKeys.PrivateKey4096 },
                };

                return data;
            }
        }

        public static TheoryData<byte[], string> InvalidByteMessages
        {
            get
            {
                var data = new TheoryData<byte[], string>
                {
                    { Faker.Random.Bytes(63), RSAKeys.PublicKey1024 },
                    { Faker.Random.Bytes(191), RSAKeys.PublicKey2048 },
                    { Faker.Random.Bytes(447), RSAKeys.PublicKey4096 },
                };

                return data;
            }
        }
    }
}