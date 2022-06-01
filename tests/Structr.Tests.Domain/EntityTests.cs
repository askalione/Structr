using Structr.Tests.Domain.TestUtils.Users;

namespace Structr.Tests.Domain
{
    public class EntityTests
    {
        [Theory]
        [InlineData(0, true)]
        [InlineData(1, false)]
        public void IsTransient(int id, bool expected)
        {
            // Arrange
            var user = new User(id, "Ivanov I.I.", new Address());

            // Act
            bool result = user.IsTransient();

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [ClassData(typeof(EqualsTheoryData))]
        public void EqualsTest(User user1, User user2, bool expected)
        {
            // Act
            bool result = user1.Equals(user2);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [ClassData(typeof(EqualsTheoryData))]
        [InlineData(null, null, true)]
        public void EqualSign(User user1, User user2, bool expected)
        {
            // Act
            bool result = user1 == user2;

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [ClassData(typeof(EqualsTheoryData))]
        [InlineData(null, null, true)]
        public void NotEqualSign(User user1, User user2, bool expected)
        {
            // Act
            bool result = user1 != user2;

            // Assert
            result.Should().NotBe(expected);
        }

        private class EqualsTheoryData : TheoryData<User, User, bool>
        {
            public EqualsTheoryData()
            {
                var user1 = new User(1, "Ivanov I.I.", new Address());
                var user2 = new User(1, "Petrov I.I.", new Address());
                var user3 = new User(3, "Ivanov I.I.", new Address());
                var user4 = new User(0, "Ivanov I.I.", new Address());
                var user5 = new User(0, "Ivanov I.I.", new Address());

                Add(user1, user1, true);
                Add(user1, user2, true);

                Add(user1, user3, false);
                Add(user1, null, false);
                Add(user4, user5, false);
            }
        }

        [Theory]
        [ClassData(typeof(GetHashCodeTheoryData))]
        public void GetHashCodeTest(User user1, User user2, bool expected)
        {
            // Arrange
            int hashCode1 = user1.GetHashCode();
            int hashCode2 = user2.GetHashCode();

            // Act
            bool result = hashCode1 == hashCode2;

            // Assert
            result.Should().Be(expected);
        }

        private class GetHashCodeTheoryData : TheoryData<User, User, bool>
        {
            public GetHashCodeTheoryData()
            {
                var user1 = new User(1, "Ivanov I.I.", new Address());
                var user2 = new User(1, "Petrov I.I.", new Address());
                var user3 = new User(3, "Ivanov I.I.", new Address());

                Add(user1, user1, true);
                Add(user1, user2, true);
                Add(user1, user3, false);
            }
        }
    }
}
