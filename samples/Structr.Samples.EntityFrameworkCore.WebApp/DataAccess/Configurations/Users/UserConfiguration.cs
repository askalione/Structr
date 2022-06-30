using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Structr.Samples.EntityFrameworkCore.WebApp.Domain.Users;

namespace Structr.Samples.EntityFrameworkCore.WebApp.DataAccess.Configurations.Users
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            builder.OwnsOne(x => x.Identity, x =>
            {
                x.Property(y => y.Gender)
                    .HasColumnType("BIT")
                    .HasConversion(
                        v => ((int)v) == 1,
                        v => v ? UserGender.Male : UserGender.Female
                    );
            });

            builder.Property(x => x.Email)
                .HasMaxLength(100);

            builder.Property(x => x.Password)
                .HasMaxLength(100);

            builder.ToTable("Users");
        }
    }
}
