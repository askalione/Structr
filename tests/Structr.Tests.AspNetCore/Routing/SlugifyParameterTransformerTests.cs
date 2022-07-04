using FluentAssertions;
using Structr.AspNetCore.Routing;
using System;
using Xunit;

namespace Structr.Tests.AspNetCore.Routing
{
    public class SlugifyParameterTransformerTests
    {
        [Theory]
        [ClassData(typeof(SlugifyParameterTransformerTheoryData))]
        public void TransformOutbound(object value, string expected)
        {
            // Act
            var result = new SlugifyParameterTransformer().TransformOutbound(value);

            // Assert
            result.Should().Be(expected);
        }

        public class SlugifyParameterTransformerTheoryData : TheoryData<object, string>
        {
            public SlugifyParameterTransformerTheoryData()
            {
                Add("/Users/AccountInfo", "/Users/Account-Info");
                Add(new DateTime(2018, 1, 18), new DateTime(2018, 1, 18).ToString());
            }
        }
    }
}
