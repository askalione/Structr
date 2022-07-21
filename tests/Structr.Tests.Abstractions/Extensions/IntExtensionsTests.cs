using FluentAssertions;
using Structr.Abstractions.Extensions;
using System.Globalization;
using Xunit;

namespace Structr.Tests.Abstractions.Extensions
{
    public class IntExtensionsTests
    {
        private class KiloFormatConvertionTheoryData : TheoryData<int, string>
        {
            public KiloFormatConvertionTheoryData()
            {
                var groupSeparator = CultureInfo.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                var decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

                Add(-17500, $"-17{groupSeparator}500");
                Add(0, $"0");
                Add(12, $"12");
                Add(2200, $"2{decimalSeparator}2K");
                Add(175223, $"175K");
                Add(520138193, $"520M");
            }
        }

        [Theory]
        [ClassData(typeof(KiloFormatConvertionTheoryData))]
        public void ToKiloFormatString(int value, string expected)
        {
            // Act
            var result = value.ToKiloFormatString();

            // Assert
            result.Should().Be(expected);
        }

        private class PluralFormConvertionTheoryData : TheoryData<int, string>
        {
            public PluralFormConvertionTheoryData()
            {
                Add(-3, "two");
                Add(-1, "one");
                Add(0, "many");
                Add(1, "one");
                Add(2, "two");
                Add(3, "two");
                Add(4, "two");
                Add(25, "many");
                Add(100, "many");
            }
        }

        [Theory]
        [ClassData(typeof(PluralFormConvertionTheoryData))]
        public void ToPluralFormString(int value, string expected)
        {
            // Act
            var result = value.ToPluralFormString("one", "two", "many");

            // Assert
            result.Should().Be(expected);
        }
    }
}