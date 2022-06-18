using FluentAssertions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using Structr.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Structr.Tests.AspNetCore.Mvc
{
    public class TempDataDictionaryExtensionsTests
    {
        public class Foo
        {
            public int Id { get; set; }
            public string? Name { get; set; }
        }

        [Fact]
        public void Put_Peek()
        {
            // Arrange
            var tempDataDictionary = GetTemDataDictionarty();
            var foo = new Foo { Id = 1, Name = "Foo" };

            // Act
            tempDataDictionary.Put("foo", foo);
            var result = tempDataDictionary.Peek<Foo>("foo");

            // Assert
            result.Should().BeEquivalentTo(foo);
        }

        [Fact]
        public void Put_overwrites_previous_value()
        {
            // Arrange
            var tempDataDictionary = GetTemDataDictionarty();
            var foo = new Foo { Id = 1, Name = "Foo" };
            var bar = new Foo { Id = 2, Name = "Bar" };

            // Act
            tempDataDictionary.Put("foo", foo);
            tempDataDictionary.Put("foo", bar);
            var result = tempDataDictionary.Peek<Foo>("foo");

            // Assert
            result.Should().BeEquivalentTo(bar);
        }

        [Fact]
        public void Peek_returns_null_if_no_key_found()
        {
            // Arrange
            var tempDataDictionary = GetTemDataDictionarty();

            // Act
            var result = tempDataDictionary.Peek<Foo>("foo");

            // Assert
            result.Should().BeNull();
        }

        private ITempDataDictionary GetTemDataDictionarty()
        {
            var context = new DefaultHttpContext();
            var tempDataProviderMock = new Mock<ITempDataProvider>();
            tempDataProviderMock.Setup(x => x.LoadTempData(context))
                .Returns(new Dictionary<string, object> { });
            return new TempDataDictionary(context, tempDataProviderMock.Object);
        }
    }
}
