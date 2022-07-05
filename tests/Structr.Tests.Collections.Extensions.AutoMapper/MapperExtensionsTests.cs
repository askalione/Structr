using AutoMapper;
using FluentAssertions;
using Structr.Collections;
using System;
using System.Collections.Generic;
using Xunit;

namespace Structr.Tests.Collections.Extensions.AutoMapper
{
    public class MapperExtensionsTests
    {
        private class Foo
        {
            public int Id { get; set; }
            public string? Name { get; set; }
        }

        private record FooDto
        {
            public int Id { get; init; }
            public string? Name { get; init; }
        }

        [Fact]
        public void MapList()
        {
            // Arrange
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Foo, FooDto>();
            });
            var mapper = configuration.CreateMapper();
            var list = new List<Foo>
            {
                 new Foo { Id = 1, Name = "Bar"},
                 new Foo { Id = 2, Name = "Baz"}
            };

            // Act
            var result = mapper.MapList<FooDto>(list);

            // Assert
            var expected = new List<FooDto>
            {
                 new FooDto { Id = 1, Name = "Bar"},
                 new FooDto { Id = 2, Name = "Baz"}
            };
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void MapList_throws_when_source_is_null()
        {
            // Arrange
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Foo, FooDto>();
            });
            var mapper = configuration.CreateMapper();
            IEnumerable<Foo> source = null!;

            // Act
            Action act = () => mapper.MapList<FooDto>(source);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void MapPagedList()
        {
            // Arrange
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Foo, FooDto>();
            });
            var mapper = configuration.CreateMapper();
            var list = new List<Foo>
            {
                 new Foo { Id = 1, Name = "Bar"},
                 new Foo { Id = 2, Name = "Baz"}
            }.ToPagedList(10, 3, 2);

            // Act
            var result = mapper.MapPagedList<FooDto>(list);

            // Assert
            var expected = new List<FooDto>
            {
                 new FooDto { Id = 1, Name = "Bar"},
                 new FooDto { Id = 2, Name = "Baz"}
            }.ToPagedList(10, 3, 2);
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void MapPagedList_throws_when_source_is_null()
        {
            // Arrange
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Foo, FooDto>();
            });
            var mapper = configuration.CreateMapper();
            PagedList<Foo> source = null!;

            // Act
            Action act = () => mapper.MapPagedList<FooDto>(source);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}