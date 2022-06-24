using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Structr.AspNetCore.Http;
using System.Collections.Generic;
using Xunit;

namespace Structr.Tests.AspNetCore.Http
{
    public class QueryCollectionExtensionsTests
    {
        [Fact]
        public void ToRouteValueDictionary()
        {
            // Arrange
            var dict = new Dictionary<string, StringValues> {
                { "Key1", new StringValues(new [] { "Value11", "Value12" }) },
                { "Key2", new StringValues(new [] { "Value2" }) }
            };

            // Act
            var result = new QueryCollection(dict).ToRouteValueDictionary();

            // Assert
            result.Should().BeEquivalentTo(dict);
        }

        // TODO: Test ToRouteValueDictionary(string, string)
    }
}
