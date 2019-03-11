using System;
using System.Diagnostics.CodeAnalysis;
using Bogus;
using Portunus.Crypto.Models;
using Portunus.Tests.Utils;
using Xunit;

namespace Portunus.Crypto.Tests.TheoryData
{
    [ExcludeFromCodeCoverage]
    internal static class TokenProviderTheoryData
    {
        private static readonly Faker Faker;

        static TokenProviderTheoryData()
        {
            Faker = new Faker();
        }

        public static TheoryData<AccessToken> AccessTokens
        {
            get
            {
                var data = new TheoryData<AccessToken>
                {
                    new AccessToken
                    {
                        ExpireOn = DateTime.UtcNow.AddMinutes(1),
                        Payload = new
                        {
                            id = long.MaxValue,
                            userName = Faker.Person.UserName,
                            email = Faker.Person.Email
                        }.ToExpandoObject()
                    },
                    new AccessToken
                    {
                        ExpireOn = DateTime.UtcNow.AddMinutes(1),
                        Payload = new
                        {
                            id = long.MaxValue,
                            userName = Faker.Person.UserName,
                            email = Faker.Person.Email,
                            name = new
                            {
                                firstName = Faker.Name.FirstName(),
                                lastName = Faker.Name.LastName()
                            }.ToExpandoObject(),
                            address = new
                            {
                                city = Faker.Address.City(),
                                country = Faker.Address.Country(),
                                latitude = Faker.Address.Latitude(),
                                longitude = Faker.Address.Longitude(),
                                state = Faker.Address.State(),
                                streetName = Faker.Address.StreetName(),
                            }.ToExpandoObject()
                        }.ToExpandoObject()
                    },
                    new AccessToken
                    {
                        ExpireOn = DateTime.UtcNow.AddMinutes(1),
                        Payload = new
                        {
                            user = new
                            {
                                id = long.MaxValue,
                                userName = Faker.Person.UserName,
                                email = Faker.Person.Email,
                                name = new
                                {
                                    firstName = Faker.Name.FirstName(),
                                    lastName = Faker.Name.LastName()
                                }.ToExpandoObject(),
                                address = new
                                {
                                    city = Faker.Address.City(),
                                    country = Faker.Address.Country(),
                                    latitude = Faker.Address.Latitude(),
                                    longitude = Faker.Address.Longitude(),
                                    state = Faker.Address.State(),
                                    streetName = Faker.Address.StreetName(),
                                }.ToExpandoObject()
                            }.ToExpandoObject()
                        }.ToExpandoObject()
                    }
                };

                return data;
            }
        }

        public static TheoryData<string> InvalidTokens
        {
            get
            {
                var rsa1024EncryptedKey = "IB76HDX5SP9GdgHzIURssod2MFXRSti7jspq6GWyHWRkYXw6_2xSC_iDOo4W42lOe2g5rTabk1xD18Z9MFa2b_clBD1q2y3HRiEwI8jkTSHF6qBPrnR1fYqny3s4cWT68pFdAMmjwCRbJdc83YKGXryr4hnCyFBosB2Ogylyodw";

                var allData = new TheoryData<string>
                {
                    null,
                    string.Empty,
                    "test",
                    "test.test.test",
                    "test.test",
                    $"{rsa1024EncryptedKey}.test"
                };

                return allData;
            }
        }
    }
}