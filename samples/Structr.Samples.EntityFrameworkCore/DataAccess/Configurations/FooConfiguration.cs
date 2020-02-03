using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Structr.Samples.EntityFrameworkCore.Domain.FooAggregate;

namespace Structr.Samples.EntityFrameworkCore.DataAccess.Configurations
{
    class FooConfiguration : IEntityTypeConfiguration<Foo>
    {
        public void Configure(EntityTypeBuilder<Foo> builder)
        {
            builder.OwnsOne(x => x.Detail);

            builder.HasMany(x => x.Items)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Foos");
        }
    }
}
