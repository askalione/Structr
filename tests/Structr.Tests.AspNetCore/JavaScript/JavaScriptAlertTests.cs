using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using Xunit;
using Structr.AspNetCore.Http;
using FluentAssertions;
using Structr.AspNetCore.JavaScript;

namespace Structr.Tests.AspNetCore.Http
{
    public class JavaScriptAlertTests
    {
        [Fact]
        public void Ctor()
        {
            // Act
            var result = new JavaScriptAlert("Type1", "Message1");

            // Assert
            result.Type.Should().Be("Type1");
            result.Message.Should().Be("Message1");
        }

        [Fact]
        public void Ctor_throws_when_type_is_empty()
        {
            // Act
            Action act = () => new JavaScriptAlert("", "Message1");

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Ctor_throws_when_message_is_empty()
        {
            // Act
            Action act = () => new JavaScriptAlert("Type1", "");

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}
