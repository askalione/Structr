using Structr.Domain;

namespace Structr.Samples.EntityFrameworkCore.WebApp.Domain.Users
{
    public class UserIdentity : ValueObject<UserIdentity>
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public DateTime DateOfBirth { get; set; }
        public UserGender Gender { get; set; }
    }
}
