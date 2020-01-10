using Xunit;

namespace Structr.Domain.Tests
{
    public class ValueObjectTests
    {
        [Fact]
        public void Equals_SetOfProperties_TrueIfAllPropertiesEqualsOtherwiseFalse()
        {
            var vo1 = new FooValueObject
            {
                Name = "Abc",
                Type = EType.Cold,
                Weight = 100
            };
            var vo2 = new FooValueObject(vo1)
            {
                Type = EType.Warm
            };
            var vo3 = new FooValueObject(vo1);

            Assert.True(vo1.Equals(vo3));
            Assert.False(vo1.Equals(vo2));
        }
    }
}
