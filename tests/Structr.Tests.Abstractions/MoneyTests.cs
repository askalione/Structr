using Structr.Abstractions;
using System;
using System.Collections.Generic;
using Xunit;
using FluentAssertions;
using System.Globalization;

namespace Structr.Tests.Abstractions
{
    public class MoneyTests
    {
        private class MyMoney : Money<long>
        {
            public MyMoney(long value) : base(value) { }
        }

        [Fact]
        public void Creation_successful()
        {
            // Act
            var result = new MyMoney(10);

            // Assert
            result.Value.Should().Be(10);
        }

        [Theory]
        [InlineData(10, true)]
        [InlineData(15, false)]
        public void Two_instances_are_equal_if_values_are_equal(long value2, bool expected)
        {
            // Act
            var result = new MyMoney(10).Equals(new MyMoney(value2));

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void Never_equals_null()
        {
            // Act
            var result = new MyMoney(10).Equals(null);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Equals_itself()
        {
            // Arrange
            var money = new MyMoney(10);

            // Act
            var result = money.Equals(money);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Equals_to_instance_of_same_type()
        {
            // Arrange
            MyMoney money1 = new MyMoney(10);
            object money2 = new MyMoney(10);

            // Act
            var result = money1.Equals(money2);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Not_equals_to_instance_of_another_type()
        {
            // Arrange
            var money1 = new MyMoney(10);
            var money2 = new Money<double>(10);

            // Act
            var result = money1.Equals(money2);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Comparision_to_other_defined_by_value_comparision()
        {
            // Act
            var result = new MyMoney(10).CompareTo(new MyMoney(15));

            // Assert
            result.Should().Be(-1);
        }

        [Fact]
        public void Cant_compare_to_instance_of_another_type()
        {
            // Act
            Action act = () => new MyMoney(10).CompareTo(new Money<double>(15));

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Comparision_to_null_gives_1()
        {
            // Act
            var result = new MyMoney(10).CompareTo(null);

            // Assert
            result.Should().Be(1);
        }

        [Fact]
        public void Comparision_to_itself_gives_0()
        {
            // Arrange
            var m = new MyMoney(10);

            // Act
            var result = m.CompareTo(m);

            // Assert
            result.Should().Be(0);
        }

        [Fact]
        public void Equal_if_have_equal_values()
        {
            // Arrange
            var money1 = new MyMoney(10);
            var money2 = new MyMoney(10);

            // Act
            var result = money1 == money2;

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Not_equal_if_have_not_equal_values()
        {
            // Arrange
            var money1 = new MyMoney(10);
            var money2 = new MyMoney(15);

            // Act
            var result = money1 != money2;

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Greater_if_has_greater_value()
        {
            // Arrange
            var money1 = new MyMoney(15);
            var money2 = new MyMoney(10);

            // Act
            var result = money1 > money2;

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Lesser_if_has_lesser_value()
        {
            // Arrange
            var money1 = new MyMoney(10);
            var money2 = new MyMoney(15);

            // Act
            var result = money1 < money2;

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Could_be_represented_as_string()
        {
            // Act
            var result = new MyMoney(15);

            // Assert
            var separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            result.ToString().Should().Be($"15{separator}00");
        }        
    }
}
