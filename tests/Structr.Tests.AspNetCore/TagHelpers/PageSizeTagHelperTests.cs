using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Moq;
using Structr.AspNetCore.TagHelpers;
using Structr.Collections;
using Structr.Tests.AspNetCore.TestUtils;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using Xunit;

namespace Structr.Tests.AspNetCore.TagHelpers
{
    public class PageSizeTagHelperTests
    {
        // TODO PageSizeTagHelperTest

        //[Fact]
        //public void Process()
        //{
        //    var context = TagHelperContextFactory.Create();
        //    var output = TagHelperOutputFactory.Create();

        //    var urlHelperMock = new Mock<IUrlHelper>();
        //    urlHelperMock.Setup(x => x.Action("TestAction", "TestController", It.IsAny<object?>()))
        //        .Returns<string?, string?, object?>((action, controller, values) => $"{controller}/{action}?" );

        //    var httpContext = new DefaultHttpContext();
        //    httpContext.Request.QueryString = new QueryString("?pagesize=4");

        //    var tagHelper = new PageSizeTagHelper(urlHelperMock.Object)
        //    {
        //        Options = new PageSizeOptions(),
        //        ViewContext = new ViewContext()
        //        {
        //            RouteData = new RouteData(new RouteValueDictionary(new Dictionary<string, string>
        //            {
        //                //{ "Area", area },
        //                { "Controller", "TestController" },
        //                { "Action", "TestAction" }
        //            }))
        //        }
        //    };

        //    // Act
        //    tagHelper.Process(context, output);

        //    // Assert
        //    var sw = new StringWriter();
        //    output.Content.WriteTo(sw, HtmlEncoder.Default);
        //    sw.ToString().Should().Be("Test Page 2 of 3. Showing items 5 through 8 of 12.");

        //    output.Attributes
        //        .FirstOrDefault(x => x.Name == "class")?
        //        .Value.Should().Be("page-info");
        //}
    }
}
