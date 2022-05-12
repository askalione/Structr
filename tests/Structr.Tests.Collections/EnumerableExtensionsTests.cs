using FluentAssertions;
using Structr.Collections;
using System;
using System.Collections.Generic;
using Xunit;

namespace Structr.Tests.Collections
{
    public class EnumerableExtensionsTests
    {
        [Fact]
        public void ToPagedList()
        {
            // Arrange
            var totalItems = 10;
            var pageNumber = 2;
            var pageSize = 3;
            var sourceCollection = new List<int> { 1, 2, 3 };

            // Act
            var result = sourceCollection.ToPagedList(totalItems, pageNumber, pageSize);

            // Assert
            result.Should().NotBeNull();
            result.TotalItems.Should().Be(totalItems);
            result.PageNumber.Should().Be(pageNumber);
            result.PageSize.Should().Be(pageSize);
            result.TotalPages.Should().Be(4); // 10/3
            result.HasPreviousPage.Should().BeTrue();
            result.HasNextPage.Should().BeTrue();
            result.IsFirstPage.Should().BeFalse();
            result.IsLastPage.Should().BeFalse();
            result.FirstItemOnPage.Should().Be(4);
            result.LastItemOnPage.Should().Be(6);
            result.Should().BeEquivalentTo(sourceCollection);
            result.Count.Should().Be(sourceCollection.Count);
        }

        [Fact]
        public void ToPagedList_without_specifying_totalItems()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 3;
            var sourceCollection = new List<int> { 1, 2, 3 };

            // Act
            var result = sourceCollection.ToPagedList(pageNumber, pageSize);

            // Assert
            result.Should().NotBeNull();
            result.TotalItems.Should().Be(3);
            result.PageNumber.Should().Be(pageNumber);
            result.PageSize.Should().Be(pageSize);
            result.TotalPages.Should().Be(1); // 3/3
            result.HasPreviousPage.Should().BeFalse();
            result.HasNextPage.Should().BeFalse();
            result.IsFirstPage.Should().BeTrue();
            result.IsLastPage.Should().BeTrue();
            result.FirstItemOnPage.Should().Be(1);
            result.LastItemOnPage.Should().Be(3);
            result.Should().BeEquivalentTo(sourceCollection);
            result.Count.Should().Be(sourceCollection.Count);
        }
    }
}