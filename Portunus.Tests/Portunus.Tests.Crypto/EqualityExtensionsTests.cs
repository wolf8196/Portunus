using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Portunus.Crypto.Extensions;
using Xunit;

namespace Portunus.Crypto.Tests
{
    [ExcludeFromCodeCoverage]
    public class EqualityExtensionsTests
    {
        [Theory]
        [InlineData("test1", "test10", false)]
        [InlineData("test1", "test1", true)]
        [InlineData("test1", "test2", false)]
        public void ComparesArrayAndSpanElementsCorrectly(string spanStr, string arrayStr, bool expected)
        {
            // Arrange
            var span = new Span<byte>(Encoding.UTF8.GetBytes(spanStr));
            var array = Encoding.UTF8.GetBytes(arrayStr);

            // Act
            var actual = span.IsEqualTo(array);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}