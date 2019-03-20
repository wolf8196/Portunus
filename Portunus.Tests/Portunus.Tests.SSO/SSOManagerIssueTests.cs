using System;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using Moq;
using Portunus.SSO.Exceptions;
using Portunus.SSO.Tests.TheoryData;
using Portunus.Tests.Shared.Extensions;
using Portunus.TokenProvider.Interfaces;
using Portunus.TokenProvider.Models;
using Portunus.TokenProvider.Settings;
using Xunit;
using static Portunus.SSO.Tests.Utils.SSOManagerTestUtils;

namespace Portunus.SSO.Tests
{
    [ExcludeFromCodeCoverage]
    public class SSOManagerIssueTests
    {
        [Theory]
        [MemberData(nameof(SSOManagerTheoryData.InvalidIssueArguments), MemberType = typeof(SSOManagerTheoryData))]
        public void ThrowsIfArgumentsAreInvalid(string app, ExpandoObject payload, Type exceptionType, string expectedParamName)
        {
            var settings = CreateSSOSettings();

            var mocks = CreateMocks(settings);

            var target = new SSOManager(mocks.factoryMock.Object, mocks.optionsMock.Object);

            var ex = Assert.Throws(exceptionType, () => target.IssueToken(app, payload));
            Assert.Equal(expectedParamName, ((ArgumentException)ex).ParamName);
        }

        [Fact]
        public void ThrowsIfRequestedAppNotRegistered()
        {
            var settings = CreateSSOSettings();

            var mocks = CreateMocks(settings);

            var target = new SSOManager(mocks.factoryMock.Object, mocks.optionsMock.Object);

            var ex = Assert.Throws<AppNotRegisteredException>(() => target.IssueToken("InvalidApp", new { }.ToExpandoObject()));

            Assert.Equal("InvalidApp", ex.AppName);
            Assert.Equal($"Requested application was not registered.{Environment.NewLine}Application name: InvalidApp", ex.Message);
        }

        [Fact]
        public void IssuesToken()
        {
            var settings = CreateSSOSettings();
            var mocks = CreateMocks(settings);
            var providerMock = new Mock<ITokenProvider>();
            providerMock.Setup(x => x.Issue(It.IsAny<AccessToken>())).Returns("TestToken");
            mocks.factoryMock.Setup(x => x.Create(It.IsAny<TokenProviderSettings>())).Returns(providerMock.Object);
            var payload = new ExpandoObject();

            var target = new SSOManager(mocks.factoryMock.Object, mocks.optionsMock.Object);

            var actual = target.IssueToken("App2", payload);

            providerMock.Verify(x => x.Issue(It.IsAny<AccessToken>()), Times.Once());
            Assert.Equal("Url2-TestToken", actual);
        }
    }
}