using System.Diagnostics.CodeAnalysis;
using Bogus;
using Xunit;

namespace Portunus.Crypto.Tests.TheoryData
{
    [ExcludeFromCodeCoverage]
    public static class AEADTheoryData
    {
        private static readonly Faker Faker;

        static AEADTheoryData()
        {
            Faker = new Faker();
        }

        public static TheoryData<byte[], byte[], string> InvalidEncryptParameters
        {
            get
            {
                var data = new TheoryData<byte[], byte[], string>
                {
                    { null, null, "plainText" },
                    { new byte[0], null, "key" }
                };

                return data;
            }
        }

        public static TheoryData<byte[], byte[], string> InvalidDecryptParameters
        {
            get
            {
                var data = new TheoryData<byte[], byte[], string>
                {
                    { null, null, "cipherText" },
                    { new byte[0], null, "key" }
                };

                return data;
            }
        }

        public static TheoryData<byte[], byte[]> ByteMessages
        {
            get
            {
                var data = new TheoryData<byte[], byte[]>
                {
                    { Faker.Random.Bytes(1), null },
                    { Faker.Random.Bytes(1), Faker.Random.Bytes(1) },
                    { Faker.Random.Bytes(16), null },
                    { Faker.Random.Bytes(16), Faker.Random.Bytes(10) },
                    { Faker.Random.Bytes(128), null },
                    { Faker.Random.Bytes(128), Faker.Random.Bytes(128) },
                    { Faker.Random.Bytes(1000), null },
                    { Faker.Random.Bytes(1000), Faker.Random.Bytes(250) },
                    { Faker.Random.Bytes(5000), null },
                    { Faker.Random.Bytes(5000), Faker.Random.Bytes(2500) }
                };

                return data;
            }
        }
    }
}