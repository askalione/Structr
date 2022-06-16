using FluentAssertions;
using Structr.AspNetCore.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Structr.Tests.AspNetCore.Json
{
    public class JsonErrorTests
    {
        [Fact]
        public void Ctor()
        {
            // Act
            var result = new JsonError("Message1");

            // Assert
            result.Message.Should().Be("Message1");
        }

        [Fact]
        public void Ctor_throws_when_message_is_empty()
        {
            // Act
            Action act = () => new JsonError("");

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Ctor_with_two()
        {
            // Act
            var result = new JsonError("Key1", "Message1");

            // Assert
            result.Key.Should().Be("Key1");
            result.Message.Should().Be("Message1");
        }

        [Fact]
        public void Ctor_with_two_throws_when_key_is_empty()
        {
            // Act
            Action act = () => new JsonError("", "Message1");

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}
