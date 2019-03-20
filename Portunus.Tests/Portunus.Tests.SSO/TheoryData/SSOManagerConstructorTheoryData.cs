using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
using Moq;
using Portunus.SSO.Settings;
using Portunus.TokenProvider.Interfaces;
using Xunit;

namespace Portunus.SSO.Tests.TheoryData
{
    [ExcludeFromCodeCoverage]
    internal static class SSOManagerConstructorTheoryData
    {
        public static TheoryData<ITokenProviderFactory, IOptions<SSOSettings>, string> InvalidConstructorArguments
        {
            get
            {
                var data = new TheoryData<ITokenProviderFactory, IOptions<SSOSettings>, string>
                {
                    { null, null, "factory" },
                    { new Mock<ITokenProviderFactory>().Object, null, "options" },
                    { new Mock<ITokenProviderFactory>().Object, new Mock<IOptions<SSOSettings>>().Object, "settings" }
                };

                return data;
            }
        }

        public static TheoryData<SSOTargetSettings, Type, string> InvalidSSOTargetSettings
        {
            get
            {
                var data = new TheoryData<SSOTargetSettings, Type, string>
                {
                    { null, typeof(ArgumentNullException), "settings" },
                    {
                        new SSOTargetSettings
                        {
                            AppName = null
                        }, typeof(ArgumentNullException), "AppName"
                    },
                    {
                        new SSOTargetSettings
                        {
                            AppName = string.Empty
                        }, typeof(ArgumentException), "AppName"
                    },
                    {
                        new SSOTargetSettings
                        {
                            AppName = "test",
                            AuthenticationUrlTemplate = null
                        }, typeof(ArgumentNullException), "AuthenticationUrlTemplate"
                    },
                    {
                        new SSOTargetSettings
                        {
                            AppName = "test",
                            AuthenticationUrlTemplate = string.Empty
                        }, typeof(ArgumentException), "AuthenticationUrlTemplate"
                    },
                    {
                        new SSOTargetSettings
                        {
                            AppName = "test",
                            AuthenticationUrlTemplate = "test",
                            TokenProviderSettings = null
                        }, typeof(ArgumentNullException), "TokenProviderSettings"
                    }
                };

                return data;
            }
        }
    }
}