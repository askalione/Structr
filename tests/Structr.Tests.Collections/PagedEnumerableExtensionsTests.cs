using FluentAssertions;
using Structr.Collections;
using System;
using System.Collections.Generic;
using Xunit;

namespace Structr.Tests.Collections
{
    public class PagedEnumerableExtensionsTests
    {
        [Fact]
        public void ToPagedList()
        {
            // Arrange
            var totalItems = 10;
            var pageNumber = 2;
            var pageSize = 3;
            var sourcePageList = new PagedList<int>(new List<int> { 1, 2, 3 },
                totalItems: totalItems,
                pageNumber: pageNumber,
                pageSize: pageSize);
            var destinationCollection = new List<int> { 4, 5, 6 };

            // Act
            var result = sourcePageList.ToPagedList(destinationCollection);

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
            result.Should().BeEquivalentTo(destinationCollection);
            result.Count.Should().Be(destinationCollection.Count);
        }

        [Fact]
        public void ToPagedList_throws_when_source_is_null()
        {
            // Arrange
            PagedList<int> sourcePageList = null!;
            var destinationCollection = new List<int> { 4, 5, 6 };

            // Act
            Action act = () => sourcePageList.ToPagedList(destinationCollection);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
