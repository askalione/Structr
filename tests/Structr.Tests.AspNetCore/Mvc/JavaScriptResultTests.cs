using FluentAssertions;
using Structr.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Structr.Tests.AspNetCore.Mvc
{
    public class JavaScriptResultTests
    {

        [Fact]
        public void Ctor()
        {
            // Act
            var result = new JavaScriptResult("alert('Hello World!')");

            // Assert
            result.ContentType.Should().Be("application/javascript");
            result.Content.Should().Be("alert('Hello World!')");
        }
    }
}
