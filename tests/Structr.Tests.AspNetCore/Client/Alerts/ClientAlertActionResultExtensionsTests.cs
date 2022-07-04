using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Structr.AspNetCore.Client.Alerts;
using Xunit;

namespace Structr.Tests.AspNetCore.Client.Alerts
{
    public class ClientAlertActionResultExtensionsTests
    {
        [Fact]
        public void AddClientAlert()
        {
            // Act
            var result = new ContentResult().AddClientAlert(new ClientAlert("Type1", "Message1"));

            // Assert
            result.Should().BeOfType<ClientAlertResult>();
        }
    }
}
