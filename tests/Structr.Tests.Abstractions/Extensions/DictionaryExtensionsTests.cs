using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;
using Structr.Abstractions.Extensions;

namespace Structr.Tests.Abstractions.Extensions
{
    public class DictionaryExtensionsTests
    {
        [Fact]
        public void AddRangeOverride()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" },
                { 3, "Three" },
                { 4, "Four" }
            };
            var newDictionary = new Dictionary<int, string>
            {
                { 1, "One_overridden" },
                { 3, "Three_overridden" },
                { 5, "Five_new" }
            };

            // Act
            dictionary.AddRangeOverride(newDictionary);

            // Assert
            var expectedDictionary = new Dictionary<int, string>
            {
                { 1, "One_overridden" },
                { 2, "Two" },
                { 3, "Three_overridden" },
                { 4, "Four" },
                { 5, "Five_new" }
            };
            dictionary.Should().BeEquivalentTo(expectedDictionary);
        }

        [Fact]
        public void AddRangeNewOnly()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" },
                { 3, "Three" },
                { 4, "Four" }
            };
            var newDictionary = new Dictionary<int, string>
            {
                { 1, "One_dont_override" },
                { 3, "Three_dont_override" },
                { 5, "Five_new" }
            };

            // Act
            dictionary.AddRangeNewOnly(newDictionary);

            // Assert
            var expectedDictionary = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" },
                { 3, "Three" },
                { 4, "Four" },
                { 5, "Five_new" }
            };
            dictionary.Should().BeEquivalentTo(expectedDictionary);
        }

        [Fact]
        public void AddRange()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" },
                { 3, "Three" },
                { 4, "Four" }
            };
            var newDictionary = new Dictionary<int, string>
            {
                { 5, "Five_new" },
                { 6, "Six_new" }
            };

            // Act
            dictionary.AddRange(newDictionary);

            // Assert
            var expectedDictionary = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" },
                { 3, "Three" },
                { 4, "Four" },
                { 5, "Five_new" },
                { 6, "Six_new" }
            };
            dictionary.Should().BeEquivalentTo(expectedDictionary);
        }

        [Fact]
        public void AddRange_throws_when_adding_existing_items()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" },
                { 3, "Three" },
                { 4, "Four" }
            };
            var newDictionary = new Dictionary<int, string>
            {
                { 1, "One_existing" },
                { 5, "Five_new" }
            };

            // Act
            Action act = () => dictionary.AddRange(newDictionary);

            // Assert
            act.Should().Throw<ArgumentException>();
        }

        [Theory]
        [ClassData(typeof(CheckingKeysTheoryData))]
        public void ContainsKeys(int[] keysToBeChecked, bool expectedResult)
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" },
                { 3, "Three" },
                { 4, "Four" }
            };

            // Act
            var result = dictionary.ContainsKeys(keysToBeChecked);

            // Assert
            result.Should().Be(expectedResult);
        }
        private class CheckingKeysTheoryData : TheoryData<int[], bool>
        {
            public CheckingKeysTheoryData()
            {
                Add(new int[] { 1, 2, 3, 4 }, true);
                Add(new int[] { 4, 5 }, true);
                Add(new int[] { 6, 7 }, false);
            }
        }
    }
}