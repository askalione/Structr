using FluentAssertions;
using Structr.Abstractions.Json.Converters;
using System;
using System.Text.Json;
using Xunit;

namespace Structr.Tests.Abstractions.Json.Converters
{
    public class DateOnlyJsonConverterTests
    {
        private class Custom
        {
            public DateOnly Date { get; set; }
        }

        [Fact]
        public void Deserialize()
        {
            // Arrange
            var options = new JsonSerializerOptions();
            options.Converters.Add(new DateOnlyJsonConverter());
            string json = "{\"Date\":\"1993-05-11\"}";

            // Act
            var result = JsonSerializer.Deserialize<Custom>(json, options);

            // Assert
            result.Should().BeEquivalentTo(new Custom { Date = new DateOnly(1993, 05, 11) });
        }

        [Fact]
        public void Serialize()
        {
            // Arrange
            var options = new JsonSerializerOptions();
            options.Converters.Add(new DateOnlyJsonConverter());
            var customObject = new Custom { Date = new DateOnly(1993, 05, 11) };

            // Act
            var result = JsonSerializer.Serialize(customObject, options);

            // Assert
            result.Should().BeEquivalentTo("{\"Date\":\"1993-05-11\"}");
        }
    }
}
