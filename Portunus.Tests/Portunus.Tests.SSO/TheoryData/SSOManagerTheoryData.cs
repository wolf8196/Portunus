using System;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using Xunit;

namespace Portunus.SSO.Tests.TheoryData
{
    [ExcludeFromCodeCoverage]
    internal static class SSOManagerTheoryData
    {
        public static TheoryData<string, ExpandoObject, Type, string> InvalidIssueArguments
        {
            get
            {
                var data = new TheoryData<string, ExpandoObject, Type, string>
                {
                    { null, null, typeof(ArgumentNullException), "app" },
                    { string.Empty, null, typeof(ArgumentException), "app" },
                    { "test", null, typeof(ArgumentNullException), "payload" },
                };

                return data;
            }
        }

        public static TheoryData<string, string, Type, string> InvalidTryVerifyArguments
        {
            get
            {
                var data = new TheoryData<string, string, Type, string>
                {
                    { null, null, typeof(ArgumentNullException), "app" },
                    { string.Empty, null, typeof(ArgumentException), "app" },
                    { "test", null, typeof(ArgumentNullException), "token" },
                    { "test", string.Empty, typeof(ArgumentException), "token" }
                };

                return data;
            }
        }
    }
}