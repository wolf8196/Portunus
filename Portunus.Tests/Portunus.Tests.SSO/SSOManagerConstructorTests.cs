using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
using Moq;
using Portunus.SSO.Settings;
using Portunus.SSO.Tests.TheoryData;
using Portunus.TokenProvider.Interfaces;
using Portunus.TokenProvider.Settings;
using Xunit;

namespace Portunus.SSO.Tests
{
    [ExcludeFromCodeCoverage]
    public class SSOManagerConstructorTests
    {
        [Theory]
        [MemberData(nameof(SSOManagerConstructorTheoryData.InvalidConstructorArguments), MemberType = typeof(SSOManagerConstructorTheoryData))]
        public void ThrowsIfArgumentsAreInvalid(
            ITokenProviderFactory factory,
            IOptions<SSOSettings> options,
            string expected)
        {
            // Arrange/Act/Assert
            var ex = Assert.Throws<ArgumentNullException>(expected, () => new SSOManager(factory, options));
        }

        [Theory]
        [MemberData(nameof(SSOManagerConstructorTheoryData.InvalidSSOTargetSettings), MemberType = typeof(SSOManagerConstructorTheoryData))]
        public void ThrowsIfSSOTargetSettingsAreInvalid(
            SSOTargetSettings settings,
            Type exceptionType,
            string expectedParamName)
        {
            // Arrange
            var targetSettings = new SSOSettings();
            targetSettings.Apps.Add(settings);

            var factoryMock = new Mock<ITokenProviderFactory>();
            var optionsMock = new Mock<IOptions<SSOSettings>>();
            optionsMock.Setup(x => x.Value).Returns(targetSettings);

            // Act/Assert
            var ex = Assert.Throws(exceptionType, () => new SSOManager(factoryMock.Object, optionsMock.Object));
            Assert.Equal(expectedParamName, ((ArgumentException)ex).ParamName);
        }

        [Fact]
        public void CreatesSSOManager()
        {
            // Arrange
            var targetSettings = new SSOSettings();
            targetSettings.Apps.Add(new SSOTargetSettings
            {
                AppName = "App1",
                AuthenticationUrlTemplate = "Url1",
                ExpireInSeconds = 1,
                TokenProviderSettings = new TokenProviderSettings()
            });
            targetSettings.Apps.Add(new SSOTargetSettings
            {
                AppName = "App2",
                AuthenticationUrlTemplate = "Url2",
                ExpireInSeconds = 2,
                TokenProviderSettings = new TokenProviderSettings()
            });

            var factoryMock = new Mock<ITokenProviderFactory>();
            var optionsMock = new Mock<IOptions<SSOSettings>>();
            optionsMock.Setup(x => x.Value).Returns(targetSettings);

            // Act
            var target = new SSOManager(factoryMock.Object, optionsMock.Object);

            // Assert
            factoryMock.Verify(x => x.Create(It.IsAny<TokenProviderSettings>()), Times.Exactly(targetSettings.Apps.Count));
        }
    }
}