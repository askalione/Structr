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
                { "Key1", "Value1" },
                { "Key2", "Value2" }
            };

            // Act
            var result = new QueryCollection(dict).ToRouteValueDictionary();

            // Assert
            result.Should().BeEquivalentTo(dict);
        }

        [Fact]
        public void ToRouteValueDictionary_with_new_values()
        {
            // Arrange
            var dict = new Dictionary<string, StringValues> {
                { "Key1", new StringValues(new[] { "Value1", "Value11" }) },
                { "Key2", new StringValues("Value2") }
            };

            // Act
            var result = new QueryCollection(dict).ToRouteValueDictionary("Key3", "Value3");

            // Assert
            result.Should().BeEquivalentTo(new Dictionary<string, object> {
                { "Key1", new StringValues(new[] { "Value1", "Value11" }) },
                { "Key2", new StringValues("Value2") },
                { "Key3", "Value3" }
            });
        }
    }
}
