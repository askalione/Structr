using FluentAssertions;
using Xunit;
using Structr.Abstractions.Extensions;
using System.Globalization;

namespace Structr.Tests.Abstractions.Extensions
{
    public class LongExtensionsTests
    {
        [Theory]
        [ClassData(typeof(CheckingConvertionTheoryData))]
        public void ToFileSizeString(long value, string expected)
        {
            // Act
            var result = value.ToFileSizeString();

            // Assert
            result.Should().Be(expected);
        }

        private class CheckingConvertionTheoryData : TheoryData<long, string>
        {
            public CheckingConvertionTheoryData()
            {
                var separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

                Add(-12, $"-12{separator}0 bytes");
                Add(0, $"0{separator}0 bytes");
                Add(12, $"12{separator}0 bytes");
                Add(2200, $"2{separator}1 KB");
                Add(3330000, $"3{separator}2 MB");
                Add(55500000000, $"51{separator}7 GB");
                Add(98700000000000, $"89{separator}8 TB");
                Add(20000000000000000, $"17{separator}8 PB");
                Add(3000000000000000000, $"2{separator}6 EB");
            }
        }
    }
}