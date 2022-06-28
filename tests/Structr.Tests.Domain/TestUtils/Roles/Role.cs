using Structr.Domain;
using System;

namespace Structr.Tests.Domain.TestUtils.Roles
{
    public class Role : Entity<Role, RoleId>, ICreatable
    {
        public string Name { get; private set; }

        public DateTime DateCreated { get; private set; }

        private Role() : base() { }

        public Role(RoleId id, string name) : this()
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Id = id;
            Name = name.Trim();
        }
    }
}
