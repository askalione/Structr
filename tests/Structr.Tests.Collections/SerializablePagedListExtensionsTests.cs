using FluentAssertions;
using Structr.Collections;
using System;
using System.Collections.Generic;
using Xunit;

namespace Structr.Tests.Collections
{
    public class SerializablePagedListExtensionsTests
    {
        [Fact]
        public void ToPagedList()
        {
            // Arrange
            var totalItems = 10;
            var pageNumber = 2;
            var pageSize = 3;
            var sourceList = new List<int> { 1, 2, 3 };
            var sourceSerializablePagedList = new SerializablePagedList<int>
            {
                Items = sourceList,
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            // Act
            var result = sourceSerializablePagedList.ToPagedList();

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
            result.Should().BeEquivalentTo(sourceList);
            result.Count.Should().Be(sourceList.Count);
        }

        [Fact]
        public void ToPagedList_throws_when_source_is_null()
        {
            // Arrange
            SerializablePagedList<int> sourceSerializablePagedList = null!;

            // Act
            Action act = () => sourceSerializablePagedList.ToPagedList();

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
