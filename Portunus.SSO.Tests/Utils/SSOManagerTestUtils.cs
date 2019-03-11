using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
using Moq;
using Portunus.Crypto.Interfaces;
using Portunus.Crypto.Models;
using Portunus.Crypto.Settings;
using Portunus.SSO.Settings;

namespace Portunus.SSO.Tests.Utils
{
    [ExcludeFromCodeCoverage]
    internal static class SSOManagerTestUtils
    {
        public delegate bool TryVerifyReturns(string s, out AccessToken token);

        public static SSOSettings CreateSSOSettings()
        {
            var settings = new SSOSettings
            {
                Apps = new List<SSOTargetSettings>
                {
                    new SSOTargetSettings
                    {
                        AppName = "App1",
                        AuthenticationUrlTemplate = "Url1-{0}",
                        ExpireInSeconds = 5,
                        TokenProviderSettings = new TokenProviderSettings()
                    },
                    new SSOTargetSettings
                    {
                        AppName = "App2",
                        AuthenticationUrlTemplate = "Url2-{0}",
                        ExpireInSeconds = 10,
                        TokenProviderSettings = new TokenProviderSettings()
                    }
                }
            };

            return settings;
        }

        public static (Mock<ITokenProviderFactory> factoryMock, Mock<IOptions<SSOSettings>> optionsMock) CreateMocks(SSOSettings settings)
        {
            var factoryMock = new Mock<ITokenProviderFactory>();
            factoryMock.Setup(x => x.Create(It.IsAny<TokenProviderSettings>())).Returns(() => new Mock<ITokenProvider>().Object);
            var optionsMock = new Mock<IOptions<SSOSettings>>();
            optionsMock.Setup(x => x.Value).Returns(settings);

            return (factoryMock, optionsMock);
        }
    }
}