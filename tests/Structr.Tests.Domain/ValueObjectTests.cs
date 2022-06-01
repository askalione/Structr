using Structr.Tests.Domain.TestUtils.Users;

namespace Structr.Tests.Domain
{
    public class ValueObjectTests
    {
        [Theory]
        [ClassData(typeof(EqualsTheoryData))]
        public void EqualsTest(Address address1, Address address2, bool expected)
        {
            // Act
            bool result = address1.Equals(address2);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [ClassData(typeof(EqualsTheoryData))]
        public void Equals_with_object(Address address1, Address address2, bool expected)
        {
            // Act
            bool result = address1.Equals((object)address2);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [ClassData(typeof(EqualsTheoryData))]
        [InlineData(null, null, true)]
        public void EqualSign(Address address1, Address address2, bool expected)
        {
            // Act
            bool result = address1 == address2;

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [ClassData(typeof(EqualsTheoryData))]
        [InlineData(null, null, true)]
        public void NotEqualSign(Address address1, Address address2, bool expected)
        {
            // Act
            bool result = address1 != address2;

            // Assert
            result.Should().NotBe(expected);
        }

        private class EqualsTheoryData : TheoryData<Address, Address, bool>
        {
            public EqualsTheoryData()
            {
                var address1 = new Address { Town = "Moscow", Street = "Prospekt Mira", House = "111" };
                var address2 = new Address { Town = "Moscow", Street = "Prospekt Mira", House = "111" };
                var address3 = new Address { Town = "Moscow", Street = "Prospekt Mira", House = "38" };

                Add(address1, address1, true);
                Add(address1, address2, true);

                Add(address1, address3, false);
                Add(address1, null, false);
            }
        }

        [Theory]
        [ClassData(typeof(GetHashCodeTheoryData))]
        public void GetHashCodeTest(Address address1, Address address2, bool expected)
        {
            // Arrange
            int hashCode1 = address1.GetHashCode();
            int hashCode2 = address2.GetHashCode();

            // Act
            bool result = hashCode1 == hashCode2;

            // Assert
            result.Should().Be(expected);
        }

        private class GetHashCodeTheoryData : TheoryData<Address, Address, bool>
        {
            public GetHashCodeTheoryData()
            {
                var address1 = new Address { Town = "Moscow", Street = "Prospekt Mira", House = "111" };
                var address2 = new Address { Town = "Moscow", Street = "Prospekt Mira", House = "111" };
                var address3 = new Address { Town = "Moscow", Street = "Prospekt Mira", House = "38" };

                Add(address1, address1, true);
                Add(address1, address2, true);
                Add(address1, address3, false);
            }
        }
    }
}
