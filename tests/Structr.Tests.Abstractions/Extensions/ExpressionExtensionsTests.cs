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
using System.Linq.Expressions;
using System.Reflection;

namespace Structr.Tests.Abstractions.Extensions
{
    public class ExpressionExtensionsTests
    {
        private class Foo
        {
            public Bar BarProperty { get; set; }

#pragma warning disable 0649
            public int BarField;
#pragma warning restore 0649

            public bool Flag { get; set; }
        }

        public class Bar
        {
            public int BarId { get; set; }
        }

        [Fact]
        public void Gets_property_name()
        {
            // Arrange
            Expression<Func<Foo, Bar>> propertyExpression = x => x.BarProperty;

            // Act
            var result = propertyExpression.GetPropertyName();

            // Assert
            result.Should().Be("BarProperty");
        }

        [Fact]
        public void Gets_nested_property_name()
        {
            // Arrange
            Expression<Func<Foo, int>> propertyExpression = x => x.BarProperty.BarId;

            // Act
            var result = propertyExpression.GetPropertyName();

            // Assert
            result.Should().Be("BarProperty.BarId");
        }

        [Fact]
        public void Getting_name_for_non_property_throws()
        {
            // Arrange
            Expression<Func<Foo, int>> propertyExpression = x => x.BarField;

            // Act
            Action act = () => propertyExpression.GetPropertyName();

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("Expression is not a property*x => x.BarField*");
        }

        [Fact]
        public void Gets_member_by_lambda_expression()
        {
            // Arrange
            Expression<Func<Foo, Bar>> propertyExpression = x => x.BarProperty;

            // Act
            var result = (propertyExpression as LambdaExpression).GetMember();

            // Assert
            result.Should().BeAssignableTo<MemberInfo>().Subject.Name.Should().Be("BarProperty");
        }

        [Fact]
        public void Gets_member_by_expression()
        {
            // Arrange
            Expression<Func<Foo, Bar>> propertyExpression = x => x.BarProperty;

            // Act
            var result = propertyExpression.GetMember();

            // Assert
            result.Should().BeAssignableTo<MemberInfo>().Subject.Name.Should().Be("BarProperty");
        }

        [Fact]
        public void Make_non_generic_Func_from_generic_one()
        {
            // Arrange
            Func<Foo, Bar> func = x => x.BarProperty;
            var foo = new Foo
            {
                BarProperty = new Bar { BarId = 1 }
            };

            // Act
            var result = func.MakeNonGeneric();

            // Assert
            result.Should().BeOfType<Func<object, object>>()
                .Subject(foo).Should().Match(x => (x as Bar).BarId == 1);
        }

        [Fact]
        public void Make_non_generic_Func_with_bool_return_from_generic_one()
        {
            // Arrange
            Func<Foo, bool> func = x => x.Flag;
            var foo = new Foo
            {
                Flag = true
            };

            // Act
            var result = func.MakeNonGeneric();

            // Assert
            result.Should().BeOfType<Func<object, bool>>()
                .Subject(foo).Should().BeTrue();
        }
    }
}