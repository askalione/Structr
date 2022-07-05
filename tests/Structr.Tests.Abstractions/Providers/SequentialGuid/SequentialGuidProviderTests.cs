using FluentAssertions;
using Structr.Abstractions.Providers.SequentialGuid;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Xunit;

namespace Structr.Tests.Abstractions.Providers.SequentialGuid
{
    public class SequentialGuidProviderTests
    {
        [Fact]
        public void Ctor()
        {
            // Act
            Action act = () => new SequentialGuidProvider(Guid.NewGuid, () => DateTime.UtcNow);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Ctor_throws_when_initializer_is_null()
        {
            // Act
            Action act = () => new SequentialGuidProvider(null, () => DateTime.UtcNow);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Ctor_throws_when_timestampProvider_is_null()
        {
            // Act
            Action act = () => new SequentialGuidProvider(Guid.NewGuid, null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [InlineData(SequentialGuidType.String)]
        [InlineData(SequentialGuidType.Binary)]
        [InlineData(SequentialGuidType.Ending)]
        [Theory]
        public void GetSequentialGuid(SequentialGuidType type)
        {
            // Arrange
            var sequentialGuidProvider = new SequentialGuidProvider(Guid.NewGuid, () => DateTime.UtcNow);

            // Act
            var result = sequentialGuidProvider.GetSequentialGuid(type);

            // Assert
            result.Should().NotBeEmpty();
        }

        [InlineData(SequentialGuidType.String)]
        [InlineData(SequentialGuidType.Binary)]
        [Theory]
        public void GetSequentialGuid_with_String_Or_Binary_type_is_sequential(SequentialGuidType type)
        {
            // Arrange
            var guidQueue = new Queue<Guid>(new[] { MaxGuid(), EmptyGuid() });
            var now = DateTime.UtcNow;
            var timestampQueue = new Queue<DateTime>(new[] { now, now.AddMilliseconds(1) });
            var sequentialGuidProvider = new SequentialGuidProvider(guidQueue.Dequeue, timestampQueue.Dequeue);

            // Act
            var guid1 = sequentialGuidProvider.GetSequentialGuid(type);
            var guid2 = sequentialGuidProvider.GetSequentialGuid(type);

            // Assert
            guid1.CompareTo(guid2).Should().Be(-1);
        }

        [Fact]
        public void GetSequentialGuid_with_Ending_type_is_sequential()
        {
            // Arrange
            var guidQueue = new Queue<Guid>(new[] { MaxGuid(), EmptyGuid() });
            var now = DateTime.UtcNow;
            var timestampQueue = new Queue<DateTime>(new[] { now, now.AddMilliseconds(1) });
            var sequentialGuidProvider = new SequentialGuidProvider(guidQueue.Dequeue, timestampQueue.Dequeue);

            // Act
            SqlGuid guid1 = sequentialGuidProvider.GetSequentialGuid(SequentialGuidType.Ending);
            SqlGuid guid2 = sequentialGuidProvider.GetSequentialGuid(SequentialGuidType.Ending);

            // Assert
            guid1.CompareTo(guid2).Should().Be(-1);
        }

        private Guid MaxGuid() =>
            new Guid(int.MaxValue,
                short.MaxValue,
                short.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue);

        private Guid EmptyGuid()
            => Guid.Empty;
    }
}
