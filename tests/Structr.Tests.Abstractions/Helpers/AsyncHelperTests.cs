using FluentAssertions;
using Structr.Abstractions.Helpers;
using System.Threading.Tasks;
using Xunit;

namespace Structr.Tests.Abstractions.Helpers
{
    public class AsyncHelperTests
    {
        private class Baz
        {
            public bool Flag { get; set; } = false;
        }

#pragma warning disable 1998
        private async Task Foo(Baz baz)
        {
            baz.Flag = true;
        }
        private async Task<int> Bar()
        {
            return 1;
        }
#pragma warning restore 1998

        [Fact]
        public void RunSync()
        {
            // Arrange
            var baz = new Baz();

            // Act
            AsyncHelper.RunSync(() => Foo(baz));

            // Assert
            baz.Flag.Should().BeTrue();
        }

        [Fact]
        public void RunSync_with_result()
        {
            // Act
            var result = AsyncHelper.RunSync(() => Bar());

            // Assert
            result.Should().Be(1);
        }
    }
}