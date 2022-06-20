using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Moq;
using Structr.AspNetCore.TagHelpers;
using Structr.Tests.AspNetCore._TestUtils;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Structr.Tests.AspNetCore.TagHelpers
{
    public class AnchorMatchTagHelperTests
    {
        [Theory]
        [InlineData("Admin", "Users", "Details", "test-css-class")]
        [InlineData("Admin", "Users", "", null)]
        [InlineData("Admin", "", "Details", null)]
        [InlineData("", "Users", "Details", null)]
        public void Process(string area, string controller, string action, string expected)
        {
            var context = TagHelperContextFactory.Create();
            var output = TagHelperOutputFactory.Create();
            var tagHelper = new AnchorMatchTagHelper(Mock.Of<IHtmlGenerator>())
            {
                MatchClass = "test-css-class",
                Area = "Admin",
                Controller = "Users",
                Action = "Details",
                ViewContext = new ViewContext()
                {
                    RouteData = new RouteData(new RouteValueDictionary(new Dictionary<string, string>
                    {
                        { "Area", area },
                        { "Controller", controller },
                        { "Action", action }
                    }))
                }
            };

            // Act
            tagHelper.Process(context, output);

            // Assert
            output.Attributes
                .FirstOrDefault(x => x.Name == "class")?
                .Value.Should().Be(expected);
        }
    }
}
