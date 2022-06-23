using Structr.Domain;
using Structr.Tests.Domain.TestUtils.Roles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Structr.Tests.Domain.TestUtils.Users
{
    public class User : Entity<User, int>
    {
        public string FullName { get; private set; }
        public Address Address { get; private set; }

        private HashSet<UserRole> _userRoles;
        public virtual IEnumerable<UserRole> UserRoles => _userRoles.ToList();

        private User() : base() { }

        public User(int id, string fullName, Address address) : this()
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                throw new ArgumentNullException(nameof(fullName));
            }
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }

            Id = id;
            FullName = fullName.Trim();
            Address = address;

            _userRoles = new HashSet<UserRole>();
        }

        public void AddRole(Role role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            var userRole = GetRole(role.Id);
            if (userRole == null)
            {
                userRole = new UserRole(this, role);
                _userRoles.Add(userRole);
            }
        }

        public UserRole GetRole(RoleId roleId)
            => _userRoles.SingleOrDefault(x => x.Role.Id == roleId);
    }
}
