using Structr.Tests.Domain.TestUtils.Roles;
using Structr.Tests.Domain.TestUtils.Users;

namespace Structr.Tests.Domain
{
    public class CompositeEntityTests
    {
        [Theory]
        [ClassData(typeof(EqualsTheoryData))]
        public void EqualsTest(UserRole userRole1, UserRole userRole2, bool expected)
        {
            // Act
            bool result = userRole1.Equals(userRole2);

            // Assert
            result.Should().Be(expected);
        }

        private class EqualsTheoryData : TheoryData<UserRole, UserRole, bool>
        {
            public EqualsTheoryData()
            {
                var user = new User(1, "Ivanov I.I.", new Address());
                var role1 = new Role(RoleId.Admin, "Admin");
                var role2 = new Role(RoleId.User, "User");

                var userRole1 = new UserRole(user, role1);
                var userRole2 = new UserRole(user, role1);
                var userRole3 = new UserRole(user, role2);

                Add(userRole1, userRole1, true);
                Add(userRole1, userRole2, true);

                Add(userRole1, userRole3, false);
                Add(userRole1, null, false);
            }
        }

        [Theory]
        [ClassData(typeof(GetHashCodeTheoryData))]
        public void GetHashCodeTest(UserRole userRole1, UserRole userRole2, bool expected)
        {
            // Arrange
            int hashCode1 = userRole1.GetHashCode();
            int hashCode2 = userRole2.GetHashCode();

            // Act
            bool result = hashCode1 == hashCode2;

            // Assert
            result.Should().Be(expected);
        }

        private class GetHashCodeTheoryData : TheoryData<UserRole, UserRole, bool>
        {
            public GetHashCodeTheoryData()
            {
                var user = new User(1, "Ivanov I.I.", new Address());
                var role1 = new Role(RoleId.Admin, "Admin");
                var role2 = new Role(RoleId.User, "User");

                var userRole1 = new UserRole(user, role1);
                var userRole2 = new UserRole(user, role1);
                var userRole3 = new UserRole(user, role2);

                Add(userRole1, userRole1, true);
                Add(userRole1, userRole2, true);
                Add(userRole1, userRole3, false);
            }
        }
    }
}
