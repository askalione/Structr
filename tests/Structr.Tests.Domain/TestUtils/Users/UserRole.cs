using Structr.Domain;
using Structr.Tests.Domain.TestUtils.Roles;
using System;
using System.Linq.Expressions;

namespace Structr.Tests.Domain.TestUtils.Users
{
    public class UserRole : CompositeEntity<UserRole>, ICreatable
    {
        public int UserId { get; private set; }
        public virtual User User { get; private set; }

        public RoleId RoleId { get; private set; }
        public virtual Role Role { get; private set; }

        public DateTime DateCreated { get; private set; }

        private UserRole() : base() { }

        internal UserRole(User user, Role role) : this()
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            UserId = user.Id;
            User = user;

            RoleId = role.Id;
            Role = role;
        }

        public static Expression<Func<UserRole, object>> CompositeId
            => x => new { x.UserId, x.RoleId };
        protected override object GetCompositeId()
            => CompositeId.Compile().Invoke(this);
    }
}
