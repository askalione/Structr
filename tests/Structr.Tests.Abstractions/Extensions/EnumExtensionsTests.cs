using FluentAssertions;
using Xunit;
using Structr.Abstractions.Extensions;
using Structr.Tests.Abstractions.TestUtils;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Structr.Tests.Abstractions.Extensions
{
    public class EnumExtensionsTests
    {
        private enum FooBarBaz
        {
            [Description("Some Foo enum")]
            Foo,

            [Display(Name = "BarBarBar")]
            Bar,

            [Display(Name = "displayNameForEnumBaz", ResourceType = typeof(TestResources))]
            [Description("Some Baz enum")]
            Baz
        }

        [Fact]
        public void GetDescription()
        {
            // Act
            var result = FooBarBaz.Foo.GetDescription();

            // Assert
            result.Should().Be("Some Foo enum");
        }

        [Fact]
        public void GetDescription_without_attribute()
        {
            // Act
            var result = FooBarBaz.Bar.GetDescription();

            // Assert
            result.Should().Be("Bar");
        }

        [Fact]
        public void GetDisplayName()
        {
            // Act
            var result = FooBarBaz.Bar.GetDisplayName();

            // Assert
            result.Should().Be("BarBarBar");
        }

        [Fact]
        public void GetDisplayName_without_attribute()
        {
            // Act
            var result = FooBarBaz.Foo.GetDisplayName();

            // Assert
            result.Should().Be("Foo");
        }

        [Fact]
        public void GetDisplayName_from_resource()
        {
            // Act
            var result = FooBarBaz.Baz.GetDisplayName();

            // Assert
            result.Should().Be("Bazzzz");
        }
    }
}