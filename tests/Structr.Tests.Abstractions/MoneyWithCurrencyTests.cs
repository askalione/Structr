using FluentAssertions;
using Structr.Abstractions;
using System;
using System.Globalization;
using Xunit;

namespace Structr.Tests.Abstractions
{
    public class MoneyWithCurrencyTests
    {
        public enum Currency
        {
            ABC,
            DEF
        }

        private class MyMoney : Money<long, Currency>
        {
            public MyMoney(long value, Currency currency) : base(value, currency) { }
        }

        [Fact]
        public void Ctor()
        {
            // Act
            var result = new MyMoney(10, Currency.ABC);

            // Assert
            result.Value.Should().Be(10);
        }

        [Theory]
        [InlineData(10, Currency.ABC, true)]
        [InlineData(10, Currency.DEF, false)]
        [InlineData(15, Currency.ABC, false)]
        public void Two_instances_are_equal_if_values_and_currencies_are_equal(long value2, Currency currency2, bool expected)
        {
            // Act
            var result = new MyMoney(10, Currency.ABC).Equals(new MyMoney(value2, currency2));

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void Never_equals_null()
        {
            // Act
            var result = new MyMoney(10, Currency.ABC).Equals(null);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Equals_itself()
        {
            // Arrange
            var money = new MyMoney(10, Currency.ABC);

            // Act
            var result = money.Equals(money);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Equals_to_instance_of_same_type()
        {
            // Arrange
            MyMoney money1 = new MyMoney(10, Currency.ABC);
            object money2 = new MyMoney(10, Currency.ABC);

            // Act
            var result = money1.Equals(money2);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Not_equals_to_instance_of_another_type()
        {
            // Arrange
            var money1 = new MyMoney(10, Currency.ABC);
            var money2 = new Money<double, Currency>(10, Currency.ABC);

            // Act
            var result = money1.Equals(money2);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Comparision_to_other_defined_by_value_comparision()
        {
            // Act
            var result = new MyMoney(10, Currency.ABC).CompareTo(new MyMoney(15, Currency.ABC));

            // Assert
            result.Should().Be(-1);
        }

        [Fact]
        public void Cant_compare_with_different_currencies()
        {
            // Act
            Action act = () => new MyMoney(10, Currency.ABC).CompareTo(new MyMoney(15, Currency.DEF));

            // Assert
            act.Should().Throw<InvalidOperationException>("Can't compare ABC and DEF.");
        }

        [Fact]
        public void Cant_compare_to_instance_of_another_type()
        {
            // Act
            Action act = () => new MyMoney(10, Currency.ABC).CompareTo(new Money<double, Currency>(15, Currency.ABC));

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Comparision_to_null_gives_1()
        {
            // Act
            var result = new MyMoney(10, Currency.ABC).CompareTo(null);

            // Assert
            result.Should().Be(1);
        }

        [Fact]
        public void Comparision_to_itself_gives_0()
        {
            // Arrange
            var m = new MyMoney(10, Currency.ABC);

            // Act
            var result = m.CompareTo(m);

            // Assert
            result.Should().Be(0);
        }

        [Fact]
        public void Equal_if_have_equal_values_and_currencies()
        {
            // Arrange
            var money1 = new MyMoney(10, Currency.ABC);
            var money2 = new MyMoney(10, Currency.ABC);

            // Act
            var result = money1 == money2;

            // Assert
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData(15, Currency.ABC)]
        [InlineData(10, Currency.DEF)]
        public void Not_equal_if_have_not_equal_values_or_currencies(long value2, Currency currency2)
        {
            // Arrange
            var money1 = new MyMoney(10, Currency.ABC);
            var money2 = new MyMoney(value2, currency2);

            // Act
            var result = money1 != money2;

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Greater_if_has_greater_value()
        {
            // Arrange
            var money1 = new MyMoney(15, Currency.ABC);
            var money2 = new MyMoney(10, Currency.ABC);

            // Act
            var result = money1 > money2;

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Lesser_if_has_lesser_value()
        {
            // Arrange
            var money1 = new MyMoney(10, Currency.ABC);
            var money2 = new MyMoney(15, Currency.ABC);

            // Act
            var result = money1 < money2;

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Could_be_represented_as_string()
        {
            // Act
            var result = new MyMoney(15, Currency.ABC);

            // Assert
            var separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            result.ToString().Should().Be($"15{separator}00 ABC");
        }
    }
}
