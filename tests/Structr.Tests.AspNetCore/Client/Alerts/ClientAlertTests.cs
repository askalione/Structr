using FluentAssertions;
using Structr.AspNetCore.Client.Alerts;
using System;
using Xunit;

namespace Structr.Tests.AspNetCore.Client.Alerts
{
    public class ClientAlertTests
    {
        [Fact]
        public void Ctor()
        {
            // Act
            var result = new ClientAlert("Type1", "Message1");

            // Assert
            result.Type.Should().Be("Type1");
            result.Message.Should().Be("Message1");
        }

        [Fact]
        public void Ctor_throws_when_type_is_empty()
        {
            // Act
            Action act = () => new ClientAlert("", "Message1");

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Ctor_throws_when_message_is_empty()
        {
            // Act
            Action act = () => new ClientAlert("Type1", "");

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}
