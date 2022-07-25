using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Structr.AspNetCore.Mvc;
using System;
using System.Text.Json;
using Xunit;

namespace Structr.Tests.AspNetCore.Mvc
{
    public class JsonOptionsExtensionsTests
    {
        private class Custom
        {
            public DateOnly Date { get; set; }
            public TimeOnly Time { get; set; }
        }

        [Fact]
        public void UseDateOnlyTimeOnlyConverters_deserialize()
        {
            // Arrange
            var options = new JsonOptions();

            // Act
            options.UseDateOnlyTimeOnlyConverters();

            // Assert
            string json = "{\"Date\":\"1993-05-11\",\"Time\":\"11:15:30.000\"}";
            var @object = JsonSerializer.Deserialize<Custom>(json, options.JsonSerializerOptions);
            @object.Should().BeEquivalentTo(new Custom { Date = new DateOnly(1993, 05, 11), Time = new TimeOnly(11, 15, 30, 0) });
        }

        [Fact]
        public void UseDateOnlyTimeOnlyConverters_serialize()
        {
            // Arrange
            var options = new JsonOptions();

            // Act
            options.UseDateOnlyTimeOnlyConverters();

            // Assert
            var @object = new Custom { Date = new DateOnly(1993, 05, 11), Time = new TimeOnly(11, 15, 30, 0) };
            var json = JsonSerializer.Serialize(@object, options.JsonSerializerOptions);
            json.Should().BeEquivalentTo("{\"Date\":\"1993-05-11\",\"Time\":\"11:15:30.000\"}");
        }
    }
}
