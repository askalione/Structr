using FluentAssertions;
using Structr.Abstractions.Providers.SequentialGuid;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Structr.Tests.Abstractions.Providers.SequentialGuid
{
    public class UniqueSequentialGuidTimestampProviderTests
    {
        private readonly ITestOutputHelper output;

        public UniqueSequentialGuidTimestampProviderTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void GetTimestamp()
        {
            // Arrange
            var size = 5;
            var timestamps = new DateTime[size];
            var timestampProvider = new UniqueSequentialGuidTimestampProvider();

            // Act
            for (int i = 0; i < size; i++)
            {
                timestamps[i] = timestampProvider.GetTimestamp();
            }

            // Assert
            timestamps.Skip(1).Select((item, i) => item.Millisecond - timestamps[i].Millisecond > UniqueSequentialGuidTimestampProvider.IncrementMs)
                .All(x => true).Should().BeTrue();
        }
    }
}
