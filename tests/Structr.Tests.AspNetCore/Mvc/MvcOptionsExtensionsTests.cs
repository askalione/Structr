using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Structr.AspNetCore.Mvc;
using Structr.AspNetCore.TypeConverters;
using System;
using System.ComponentModel;
using Xunit;

namespace Structr.Tests.AspNetCore.Mvc
{
    public class MvcOptionsExtensionsTests
    {
        [Fact]
        public void UseDateOnlyTimeOnlyConverters()
        {
            // Arrange
            var options = new MvcOptions();

            // Act
            options.UseDateOnlyTimeOnlyConverters();

            // Assert
            AttributeCollection attributes;
            TypeConverterAttribute? attribute;

            attributes = TypeDescriptor.GetAttributes(typeof(DateOnly));
            attribute = (TypeConverterAttribute?)attributes[typeof(TypeConverterAttribute)];
            attribute!.ConverterTypeName.Should().StartWith(typeof(DateOnlyTypeConverter).FullName);

            attributes = TypeDescriptor.GetAttributes(typeof(TimeOnly));
            attribute = (TypeConverterAttribute?)attributes[typeof(TypeConverterAttribute)];
            attribute!.ConverterTypeName.Should().StartWith(typeof(TimeOnlyTypeConverter).FullName);
        }
    }
}
