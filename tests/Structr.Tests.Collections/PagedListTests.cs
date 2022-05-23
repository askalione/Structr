using FluentAssertions;
using Structr.Collections;
using System;
using System.Collections.Generic;
using Xunit;

namespace Structr.Tests.Collections
{
    public class PagedListTests
    {
        [Fact]
        public void Ctor()
        {
            // Arrange
            var totalItems = 10;
            var pageNumber = 2;
            var pageSize = 3;
            var sourceCollection = new List<int> { 1, 2, 3 };

            // Act
            var result = new PagedList<int>(sourceCollection, totalItems, pageNumber, pageSize);

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
        public void Ctor_throws_when_collection_is_null()
        {
            // Arrange
            List<int>? sourceCollection = null;

            // Act
            Action act = () => new PagedList<int>(sourceCollection, totalItems: 1, pageNumber: 1, pageSize: 1);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Ctor_throws_when_totalItems_less_than_0()
        {
            // Arrange
            var sourceCollection = new List<int> { 1, 2, 3 };

            // Act
            Action act = () => new PagedList<int>(sourceCollection, totalItems: -1, pageNumber: 1, pageSize: 1);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>().WithMessage("Total number of elements in superset must be greater or equal 0*totalItems*");
        }

        [Fact]
        public void Ctor_throws_when_totalItems_less_than_source_collection_count()
        {
            // Arrange
            var sourceCollection = new List<int> { 1, 2, 3 };

            // Act
            Action act = () => new PagedList<int>(sourceCollection, totalItems: 1, pageNumber: 1, pageSize: 3);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>().WithMessage("Total number of elements in superset must be greater or equal collection items count*totalItems*");
        }

        [Fact]
        public void Ctor_throws_when_pageNumber_less_than_1()
        {
            // Arrange
            var sourceCollection = new List<int> { 1, 2, 3 };

            // Act
            Action act = () => new PagedList<int>(sourceCollection, totalItems: 10, pageNumber: 0, pageSize: 3);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>().WithMessage("Page number must be greater or equal 1*pageNumber*");
        }

        [Fact]
        public void Ctor_throws_when_pageSize_less_than_0()
        {
            // Arrange
            var sourceCollection = new List<int> { 1, 2, 3 };

            // Act
            Action act = () => new PagedList<int>(sourceCollection, totalItems: 10, pageNumber: 1, pageSize: -1);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>().WithMessage("Page size must be greater or equal 0*pageSize*");
        }

        [Fact]
        public void Ctor_throws_when_pageSize_less_than_source_collection_count()
        {
            // Arrange
            var sourceCollection = new List<int> { 1, 2, 3 };

            // Act
            Action act = () => new PagedList<int>(sourceCollection, totalItems: 10, pageNumber: 1, pageSize: 1);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>().WithMessage("Page size must be greater or equal collection items count*pageSize*");
        }

        [Fact]
        public void Empty()
        {
            // Act
            var result = PagedList.Empty<int>();

            // Assert
            result.Should().NotBeNull();
            result.TotalItems.Should().Be(0);
            result.PageNumber.Should().Be(1);
            result.PageSize.Should().Be(0);
            result.TotalPages.Should().Be(0);
            result.HasPreviousPage.Should().BeFalse();
            result.HasNextPage.Should().BeFalse();
            result.IsFirstPage.Should().BeFalse();
            result.IsLastPage.Should().BeFalse();
            result.FirstItemOnPage.Should().Be(0);
            result.LastItemOnPage.Should().Be(0);
            result.Should().BeEquivalentTo(new List<int>());
            result.Count.Should().Be(0);
        }
    }
}