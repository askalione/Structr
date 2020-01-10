using System;
using Xunit;

namespace Structr.Domain.Tests
{
    public class EntityTests
    {
        [Fact]
        public void Ctr_EntityOfWrongGenericType_ThrownException()
        {
            var ex = Assert.Throws<InvalidOperationException>(() => new BarEntity(1, EType.Cold));
            Assert.Equal(
                $"Entity '{typeof(BarEntity)}' specifies '{typeof(FooEntity).Name}' as generic argument, it should be its own type", ex.Message);
        }

        [Theory]
        [InlineData(10, 10)]
        [InlineData(100, 102)]        
        public void Equals_KeyPairs_KeyPairsEquals(int id1, int id2)
        {
            var entity1 = new FooEntity(id1);
            var entity2 = new FooEntity(id2);

            Assert.Equal(entity1.Equals(entity2), id1.Equals(id2));
        }
    }
}
