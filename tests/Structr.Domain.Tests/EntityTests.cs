using System;
using Xunit;

namespace Structr.Domain.Tests
{
    public class EntityTests
    {
        //[Theory]
        //[InlineData(10, 10)]
        //[InlineData(100, 102)]        
        //[InlineData("key-1", "key-1")]        
        //[InlineData("key-1", "key-2")]
        //[InlineData(EType.Cold, EType.Cold)]        
        //[InlineData(EType.Cold, EType.Warm)]
        [Fact]
        public void Equals_KeyPairs_KeyPairsEquals()
        {
            var entity1 = new FooEntity(10);
            var entity2 = new FooEntity(10);
            Assert.True(entity1.Equals(entity2));

            entity1 = new FooEntity(10);
            entity2 = new FooEntity(11);
            Assert.False(entity1.Equals(entity2));

            entity1 = new FooEntity("key-1");
            entity2 = new FooEntity("key-1");
            Assert.True(entity1.Equals(entity2));

            entity1 = new FooEntity("key-1");
            entity2 = new FooEntity("key-2");
            Assert.False(entity1.Equals(entity2));

            entity1 = new FooEntity(new Guid("84765ea9-c41e-45e1-bd8b-04e9df1cb804"));
            entity2 = new FooEntity(new Guid("84765ea9-c41e-45e1-bd8b-04e9df1cb804"));
            Assert.True(entity1.Equals(entity2));

            entity1 = new FooEntity(new Guid("84765ea9-c41e-45e1-bd8b-04e9df1cb804"));
            entity2 = new FooEntity(new Guid("e94b119c-5815-4aa8-b558-45f2685c58c8"));
            Assert.False(entity1.Equals(entity2));

            entity1 = new FooEntity(Type.Cold);
            entity2 = new FooEntity(Type.Cold);
            Assert.True(entity1.Equals(entity2));

            entity1 = new FooEntity(Type.Cold);
            entity2 = new FooEntity(Type.Warm);
            Assert.False(entity1.Equals(entity2));

            entity1 = new FooEntity(new Id(1, 1));
            entity2 = new FooEntity(new Id(1, 1));
            Assert.True(entity1.Equals(entity2));

            entity1 = new FooEntity(new Id(1, 1));
            entity2 = new FooEntity(new Id(2, 1));
            Assert.False(entity1.Equals(entity2));
        }
    }
}
