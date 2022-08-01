using FluentAssertions;
using Structr.Abstractions.Json.Converters;
using System.Text.Json;
using Xunit;

namespace Structr.Tests.Abstractions.Json.Converters
{
    public class StringNumberJsonConverterTests
    {
        private class Custom
        {
            public string Value { get; set; }
        }

        [Fact]
        public void Deserialize()
        {
            // Arrange
            var options = new JsonSerializerOptions();
            options.Converters.Add(new StringNumberJsonConverter());
            string json = "{\"Value\":123}";

            // Act
            var result = JsonSerializer.Deserialize<Custom>(json, options);

            // Assert
            result.Should().BeEquivalentTo(new Custom { Value = "123" });
        }

        [Fact]
        public void Serialize()
        {
            // Arrange
            var options = new JsonSerializerOptions();
            options.Converters.Add(new StringNumberJsonConverter());
            var customObject = new Custom { Value = "123" };

            // Act
            var result = JsonSerializer.Serialize(customObject, options);

            // Assert
            result.Should().BeEquivalentTo("{\"Value\":\"123\"}");
        }
    }
}
