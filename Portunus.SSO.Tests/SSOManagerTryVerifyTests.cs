using System;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using Moq;
using Portunus.Crypto.Interfaces;
using Portunus.Crypto.Models;
using Portunus.Crypto.Settings;
using Portunus.SSO.Exceptions;
using Portunus.SSO.Tests.TheoryData;
using Portunus.Tests.Utils;
using Xunit;
using static Portunus.SSO.Tests.Utils.SSOManagerTestUtils;

namespace Portunus.SSO.Tests
{
    [ExcludeFromCodeCoverage]
    public class SSOManagerTryVerifyTests
    {
        [Theory]
        [MemberData(nameof(SSOManagerTheoryData.InvalidTryVerifyArguments), MemberType = typeof(SSOManagerTheoryData))]
        public void ThrowsIfArgumentsAreInvalid(string app, string token, Type exceptionType, string expectedParamName)
        {
            var settings = CreateSSOSettings();

            var mocks = CreateMocks(settings);

            var target = new SSOManager(mocks.factoryMock.Object, mocks.optionsMock.Object);

            var ex = Assert.Throws(exceptionType, () => target.TryVerifyToken(app, token, out ExpandoObject payload));
            Assert.Equal(expectedParamName, ((ArgumentException)ex).ParamName);
        }

        [Fact]
        public void ThrowsIfRequestedAppNotRegistered()
        {
            var settings = CreateSSOSettings();

            var mocks = CreateMocks(settings);

            var target = new SSOManager(mocks.factoryMock.Object, mocks.optionsMock.Object);

            var ex = Assert.Throws<AppNotRegisteredException>(() => target.TryVerifyToken("InvalidApp", "TestToken", out ExpandoObject payload));

            Assert.Equal("InvalidApp", ex.AppName);
        }

        [Fact]
        public void RejectsWhenTokenIsInvalid()
        {
            var settings = CreateSSOSettings();
            var providerMock = new Mock<ITokenProvider>();
            var mocks = CreateMocks(settings);

            providerMock
                .Setup(x => x.TryVerify(It.IsAny<string>(), out It.Ref<AccessToken>.IsAny))
                .Returns(new TryVerifyReturns((string s, out AccessToken token) =>
                {
                    token = null;
                    return false;
                }));

            mocks.factoryMock
                .Setup(x => x.Create(It.IsAny<TokenProviderSettings>()))
                .Returns(providerMock.Object);

            var target = new SSOManager(mocks.factoryMock.Object, mocks.optionsMock.Object);

            var actual = target.TryVerifyToken("App1", "TestToken", out ExpandoObject payload);

            Assert.False(actual);
            Assert.Null(payload);
        }

        [Fact]
        public void RejectsWhenTokenIsExpired()
        {
            var settings = CreateSSOSettings();
            var providerMock = new Mock<ITokenProvider>();
            var mocks = CreateMocks(settings);
            var testPayload = new { id = 1 }.ToExpandoObject();

            providerMock
                .Setup(x => x.TryVerify(It.IsAny<string>(), out It.Ref<AccessToken>.IsAny))
                .Returns(new TryVerifyReturns((string s, out AccessToken token) =>
                {
                    token = new AccessToken
                    {
                        ExpireOn = DateTime.UtcNow.AddMilliseconds(-1),
                        Payload = testPayload
                    };
                    return true;
                }));

            mocks.factoryMock
                .Setup(x => x.Create(It.IsAny<TokenProviderSettings>()))
                .Returns(providerMock.Object);

            var target = new SSOManager(mocks.factoryMock.Object, mocks.optionsMock.Object);

            var actual = target.TryVerifyToken("App1", "TestToken", out ExpandoObject payload);

            Assert.False(actual);
            Assert.Null(payload);
        }

        [Fact]
        public void VerifiesToken()
        {
            var settings = CreateSSOSettings();
            var mocks = CreateMocks(settings);
            var providerMock = new Mock<ITokenProvider>();
            var expectedPayload = new { id = 1 }.ToExpandoObject();

            providerMock
                .Setup(x => x.TryVerify(It.IsAny<string>(), out It.Ref<AccessToken>.IsAny))
                .Returns(new TryVerifyReturns((string s, out AccessToken token) =>
                {
                    token = new AccessToken
                    {
                        ExpireOn = DateTime.UtcNow.AddMonths(1),
                        Payload = expectedPayload
                    };

                    return true;
                }));

            mocks.factoryMock
                .Setup(x => x.Create(It.IsAny<TokenProviderSettings>()))
                .Returns(providerMock.Object);

            var target = new SSOManager(mocks.factoryMock.Object, mocks.optionsMock.Object);

            var actual = target.TryVerifyToken("App2", "TestToken", out ExpandoObject actualPayload);

            Assert.True(actual);
            Assert.Equal(expectedPayload, actualPayload);
            providerMock.Verify(x => x.TryVerify(It.IsAny<string>(), out It.Ref<AccessToken>.IsAny), Times.Once());
        }
    }
}