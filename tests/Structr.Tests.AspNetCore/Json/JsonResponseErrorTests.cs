using FluentAssertions;
using Structr.AspNetCore.Json;
using System;
using Xunit;

namespace Structr.Tests.AspNetCore.Json
{
    public class JsonResponseErrorTests
    {
        [Fact]
        public void Ctor()
        {
            // Act
            var result = new JsonResponseError("Message1");

            // Assert
            result.Message.Should().Be("Message1");
        }

        [Fact]
        public void Ctor_throws_when_message_is_empty()
        {
            // Act
            Action act = () => new JsonResponseError("");

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Ctor_with_two()
        {
            // Act
            var result = new JsonResponseError("Key1", "Message1");

            // Assert
            result.Key.Should().Be("Key1");
            result.Message.Should().Be("Message1");
        }

        [Fact]
        public void Ctor_with_two_throws_when_key_is_empty()
        {
            // Act
            Action act = () => new JsonResponseError("", "Message1");

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}
