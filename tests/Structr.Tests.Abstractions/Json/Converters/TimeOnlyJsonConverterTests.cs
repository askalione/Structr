using FluentAssertions;
using Structr.Abstractions.Json.Converters;
using System;
using System.Text.Json;
using Xunit;

namespace Structr.Tests.Abstractions.Json.Converters
{
    public class TimeOnlyJsonConverterTests
    {
        private class Custom
        {
            public TimeOnly Time { get; set; }
        }

        [Fact]
        public void Deserialize()
        {
            // Arrange
            var options = new JsonSerializerOptions();
            options.Converters.Add(new TimeOnlyJsonConverter());
            string json = "{\"Time\":\"11:15:30.000\"}";

            // Act
            var result = JsonSerializer.Deserialize<Custom>(json, options);

            // Assert
            result.Should().BeEquivalentTo(new Custom { Time = new TimeOnly(11, 15, 30, 0) });
        }

        [Fact]
        public void Serialize()
        {
            // Arrange
            var options = new JsonSerializerOptions();
            options.Converters.Add(new TimeOnlyJsonConverter());
            var customObject = new Custom { Time = new TimeOnly(11, 15, 30, 0) };

            // Act
            var result = JsonSerializer.Serialize(customObject, options);

            // Assert
            result.Should().BeEquivalentTo("{\"Time\":\"11:15:30.000\"}");
        }
    }
}
