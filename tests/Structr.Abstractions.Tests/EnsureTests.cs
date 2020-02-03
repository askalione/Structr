using System;
using Xunit;

namespace Structr.Abstractions.Tests
{
    public class EnsureTests
    {
        [Fact]
        public void NotNull_NullValue_ThrowArgumentNullException()
        {
            object value = null;
            string name = nameof(value);
            var actual = new ArgumentNullException(name);

            var expected = Assert.Throws<ArgumentNullException>(() => Ensure.NotNull(value, name));

            Assert.Equal(actual.Message, expected.Message);
        }

        [Fact]
        public void NotEmpty_EmptyString_ThrowArgumentNullException()
        {
            string value = "";
            string name = nameof(value);
            var actual = new ArgumentNullException(name);

            var expected = Assert.Throws<ArgumentNullException>(() => Ensure.NotEmpty(value, name));

            Assert.Equal(actual.Message, expected.Message);
        }

        [Fact]
        public void InRange_OutOfRangeStringLength_ThrowArgumentOutOfRangeException()
        {
            string value = "Creacode";
            string name = nameof(value);
            var minLength = 3;
            var maxLength = 7;
            var actual = new ArgumentOutOfRangeException(name, value,
                $"String length is out of range. The length must be between {minLength} and {maxLength}.");

            var expected = Assert.Throws<ArgumentOutOfRangeException>(() => Ensure.InRange(value, minLength, maxLength, name));

            Assert.Equal(actual.Message, expected.Message);
        }

        [Fact]
        public void InRange_OutOfRangeDate_ThrowArgumentOutOfRangeException()
        {
            DateTime value = DateTime.Now;
            string name = nameof(value);
            var minValue = value.AddDays(-10);
            var maxValue = value.AddDays(-2);
            var actual = new ArgumentOutOfRangeException(name, value,
                $"Value is out of range. Value must be between {minValue} and {maxValue}.");

            var expected = Assert.Throws<ArgumentOutOfRangeException>(() => Ensure.InRange(value, minValue, maxValue, name));

            Assert.Equal(actual.Message, expected.Message);
        }

        [Fact]
        public void InRange_OutOfRangeInt_ThrowArgumentOutOfRangeException()
        {
            int value = 100;
            string name = nameof(value);
            var minValue = value - value;
            var maxValue = value - value / 2;
            var actual = new ArgumentOutOfRangeException(name, value,
                $"Value is out of range. Value must be between {minValue} and {maxValue}.");

            var expected = Assert.Throws<ArgumentOutOfRangeException>(() => Ensure.InRange(value, minValue, maxValue, name));

            Assert.Equal(actual.Message, expected.Message);
        }

        [Fact]
        public void GreaterThan_OutOfRangeStringLength_ThrowArgumentOutOfRangeException()
        {
            string value = "Creacode";
            string name = nameof(value);
            var minLength = value.Length + 1;
            var actual = new ArgumentOutOfRangeException(name, value,
                $"String length is out of range. The length must be greater than {minLength}.");

            var expected = Assert.Throws<ArgumentOutOfRangeException>(() => Ensure.GreaterThan(value, minLength, name));

            Assert.Equal(actual.Message, expected.Message);
        }

        [Fact]
        public void GreaterThan_OutOfRangeDate_ThrowArgumentOutOfRangeException()
        {
            DateTime value = DateTime.Now;
            string name = nameof(value);
            var minValue = value.AddDays(1);
            var actual = new ArgumentOutOfRangeException(name, value,
                $"Value is out of range. Value must be greater than {minValue}.");

            var expected = Assert.Throws<ArgumentOutOfRangeException>(() => Ensure.GreaterThan(value, minValue, name));

            Assert.Equal(actual.Message, expected.Message);
        }

        [Fact]
        public void GreaterThan_OutOfRangeDecimal_ThrowArgumentOutOfRangeException()
        {
            decimal value = 100;
            string name = nameof(value);
            var minValue = value + 50;
            var actual = new ArgumentOutOfRangeException(name, value,
                $"Value is out of range. Value must be greater than {minValue}.");

            var expected = Assert.Throws<ArgumentOutOfRangeException>(() => Ensure.GreaterThan(value, minValue, name));

            Assert.Equal(actual.Message, expected.Message);
        }

        [Fact]
        public void LessThan_OutOfRangeStringLength_ThrowArgumentOutOfRangeException()
        {
            string value = "Creacode";
            string name = nameof(value);
            var maxLength = value.Length - 1;
            var actual = new ArgumentOutOfRangeException(name, value,
                $"String length is out of range. The length must be less than {maxLength}.");

            var expected = Assert.Throws<ArgumentOutOfRangeException>(() => Ensure.LessThan(value, maxLength, name));

            Assert.Equal(actual.Message, expected.Message);
        }

        [Fact]
        public void LessThan_OutOfRangeDate_ThrowArgumentOutOfRangeException()
        {
            DateTime value = DateTime.Now;
            string name = nameof(value);
            var maxValue = value.AddDays(-1);
            var actual = new ArgumentOutOfRangeException(name, value,
                 $"Value is out of range. Value must be less than {maxValue}.");

            var expected = Assert.Throws<ArgumentOutOfRangeException>(() => Ensure.LessThan(value, maxValue, name));

            Assert.Equal(actual.Message, expected.Message);
        }

        [Fact]
        public void LessThan_OutOfRangeDouble_ThrowArgumentOutOfRangeException()
        {
            double value = 100.1;
            string name = nameof(value);
            var maxValue = value - value / 2;
            var actual = new ArgumentOutOfRangeException(name, value,
                 $"Value is out of range. Value must be less than {maxValue}.");

            var expected = Assert.Throws<ArgumentOutOfRangeException>(() => Ensure.LessThan(value, maxValue, name));

            Assert.Equal(actual.Message, expected.Message);
        }
    }
}
