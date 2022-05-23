using FluentAssertions;
using Structr.Abstractions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Structr.Tests.Abstractions
{
    public class EnsureTests
    {
        [Theory]
        [InlineData(null, false)]
        [InlineData("123456", true)]
        public void NotNull(object myValue, bool passes)
        {
            // Act
            Action act = () => Ensure.NotNull(myValue, nameof(myValue));

            // Assert
            if (passes)
            {
                act.Should().NotThrow();
            }
            else
            {
                act.Should().Throw<ArgumentNullException>()
                    .WithMessage("*myValue*");
            }
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("123456", true)]
        public void NotEmpty(string myValue, bool passes)
        {
            // Act
            Action act = () => Ensure.NotEmpty(myValue, nameof(myValue));

            // Assert
            if (passes)
            {
                act.Should().NotThrow();
            }
            else
            {
                act.Should().Throw<ArgumentNullException>()
                    .WithMessage("*myValue*");
            }
        }

        [Theory]
        [ClassData(typeof(NotEmptyTheoryData))]
        public void NotEmpty_for_collection(IEnumerable<int> myValue, bool passes)
        {
            // Act
            Action act = () => Ensure.NotEmpty(myValue, nameof(myValue));

            // Assert
            if (passes)
            {
                act.Should().NotThrow();
            }
            else
            {
                act.Should().Throw<ArgumentNullException>()
                    .WithMessage("*myValue*");
            }
        }
        private class NotEmptyTheoryData : TheoryData<IEnumerable<int>, bool>
        {
            public NotEmptyTheoryData()
            {
                Add(null, false);
                Add(new int[] { }, false);
                Add(new int[] { 1, 2, 3 }, true);
            }
        }

        [Theory]
        [InlineData("123", false)]
        [InlineData("123456", true)]
        [InlineData("123456789", false)]
        public void InRange_for_String(string myValue, bool passes)
        {
            // Act
            Action act = () => Ensure.InRange(myValue, 4, 8, nameof(myValue));

            // Assert
            if (passes)
            {
                act.Should().NotThrow();
            }
            else
            {
                act.Should().Throw<ArgumentOutOfRangeException>()
                    .WithMessage("String length is out of range. The length must be between 4 and 8.*myValue*");
            }
        }

        [Theory]
        [InlineData("1980-01-01", false)]
        [InlineData("2000-05-30", true)]
        [InlineData("2023-05-09", false)]
        public void InRange_for_DateTime(string value, bool passes)
        {
            // Arrange
            var myValue = DateTime.Parse(value);
            var minValue = DateTime.Parse("1990-01-01");
            var maxValue = DateTime.Parse("2022-12-31");

            // Act
            Action act = () => Ensure.InRange(myValue, minValue, maxValue, nameof(myValue));

            // Assert
            if (passes)
            {
                act.Should().NotThrow();
            }
            else
            {
                act.Should().Throw<ArgumentOutOfRangeException>()
                    .WithMessage($"Value is out of range. Value must be between {minValue} and {maxValue}.*myValue*");
            }
        }

        [Theory]
        [InlineData(3, false)]
        [InlineData(5, true)]
        [InlineData(9, false)]
        public void InRange(IComparable myValue, bool passes)
        {
            // Act
            Action act = () => Ensure.InRange(myValue, 4, 8, nameof(myValue));

            // Assert
            if (passes)
            {
                act.Should().NotThrow();
            }
            else
            {
                act.Should().Throw<ArgumentOutOfRangeException>()
                    .WithMessage("Value is out of range. Value must be between 4 and 8.*myValue*");
            }
        }

        [Theory]
        [InlineData("123", false)]
        [InlineData("123456", true)]
        public void GreaterThan_for_String(string myValue, bool passes)
        {
            // Act
            Action act = () => Ensure.GreaterThan(myValue, 4, nameof(myValue));

            // Assert
            if (passes)
            {
                act.Should().NotThrow();
            }
            else
            {
                act.Should().Throw<ArgumentOutOfRangeException>()
                    .WithMessage("String length is out of range. The length must be greater than 4.*myValue*");
            }
        }

        [Theory]
        [InlineData("1980-01-01", false)]
        [InlineData("2000-05-30", true)]
        public void GreaterThan_for_DateTime(string value, bool passes)
        {
            // Arrange
            var myValue = DateTime.Parse(value);
            var minValue = DateTime.Parse("1990-01-01");

            // Act
            Action act = () => Ensure.GreaterThan(myValue, minValue, nameof(myValue));

            // Assert
            if (passes)
            {
                act.Should().NotThrow();
            }
            else
            {
                act.Should().Throw<ArgumentOutOfRangeException>()
                    .WithMessage($"Value is out of range. Value must be greater than {minValue}.*myValue*");
            }
        }

        [Theory]
        [InlineData(3, false)]
        [InlineData(5, true)]
        public void GreaterThan(IComparable myValue, bool passes)
        {
            // Act
            Action act = () => Ensure.GreaterThan(myValue, 4, nameof(myValue));

            // Assert
            if (passes)
            {
                act.Should().NotThrow();
            }
            else
            {
                act.Should().Throw<ArgumentOutOfRangeException>()
                    .WithMessage("Value is out of range. Value must be greater than 4.*myValue*");
            }
        }

        [Theory]
        [InlineData("123456", true)]
        [InlineData("123456789", false)]
        public void LessThan_for_String(string myValue, bool passes)
        {
            // Act
            Action act = () => Ensure.LessThan(myValue, 8, nameof(myValue));

            // Assert
            if (passes)
            {
                act.Should().NotThrow();
            }
            else
            {
                act.Should().Throw<ArgumentOutOfRangeException>()
                    .WithMessage("String length is out of range. The length must be less than 8.*myValue*");
            }
        }

        [Theory]
        [InlineData("2000-05-30", true)]
        [InlineData("2023-05-09", false)]
        public void LessThan_for_DateTime(string value, bool passes)
        {
            // Arrange
            var myValue = DateTime.Parse(value);
            var maxValue = DateTime.Parse("2022-12-31");

            // Act
            Action act = () => Ensure.LessThan(myValue, maxValue, nameof(myValue));

            // Assert
            if (passes)
            {
                act.Should().NotThrow();
            }
            else
            {
                act.Should().Throw<ArgumentOutOfRangeException>()
                    .WithMessage($"Value is out of range. Value must be less than {maxValue}.*myValue*");
            }
        }

        [Theory]
        [InlineData(5, true)]
        [InlineData(9, false)]
        public void LessThan(IComparable myValue, bool passes)
        {
            // Act
            Action act = () => Ensure.LessThan(myValue, 8, nameof(myValue));

            // Assert
            if (passes)
            {
                act.Should().NotThrow();
            }
            else
            {
                act.Should().Throw<ArgumentOutOfRangeException>()
                    .WithMessage("Value is out of range. Value must be less than 8.*myValue*");
            }
        }
    }
}
