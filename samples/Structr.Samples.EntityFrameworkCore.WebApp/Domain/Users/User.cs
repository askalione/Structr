using Structr.Domain;

namespace Structr.Samples.EntityFrameworkCore.WebApp.Domain.Users
{
    public class User : Entity<User, int>
    {
        public UserIdentity Identity { get; private set; } = default!;
        public string Email { get; private set; } = default!;
        public string Password { get; private set; } = default!;

        private User() : base() { }

        public User(UserIdentity identity, string email, string password)
        {
            Identity = identity;
            Email = email;
            Password = password;
        }
    }
}
