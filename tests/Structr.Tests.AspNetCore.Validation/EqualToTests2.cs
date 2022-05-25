using Xunit;
using FluentAssertions;
using Structr.AspNetCore.Validation;

namespace Structr.Tests.AspNetCore.Validation
{
    public class EqualToTests2
    {
        private class EqualTo_Model
        {
            public int Value1 { get; set; }
            public int Value2 { get; set; }
        }

        [Fact]
        public void EqualTo()
        {
            // Arrange
            var model = new EqualTo_Model { Value1 = 1, Value2 = 1 };
            var attribute = new EqualToAttribute(nameof(EqualTo_Model.Value2));

            // Act
            var result = attribute.IsValid(model.Value1, model);

            // Assert
            result.Should().BeTrue();
        }
    }
}