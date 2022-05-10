using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Structr.Abstractions.Extensions;
using System.IO;
using Structr.Tests.Abstractions.TestsInfrastructure;
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
        public void Gets_description()
        {
            // Act
            var result = FooBarBaz.Foo.GetDescription();

            // Assert
            result.Should().Be("Some Foo enum");
        }

        [Fact]
        public void Gets_description_without_attribute()
        {
            // Act
            var result = FooBarBaz.Bar.GetDescription();

            // Assert
            result.Should().Be("Bar");
        }

        [Fact]
        public void Gets_displayName()
        {
            // Act
            var result = FooBarBaz.Bar.GetDisplayName();

            // Assert
            result.Should().Be("BarBarBar");
        }

        [Fact]
        public void Gets_displayName_without_attribute()
        {
            // Act
            var result = FooBarBaz.Foo.GetDisplayName();

            // Assert
            result.Should().Be("Foo");
        }

        [Fact]
        public void Gets_displayName_from_resource()
        {
            // Act
            var result = FooBarBaz.Baz.GetDisplayName();

            // Assert
            result.Should().Be("Bazzzz");
        }
    }
}