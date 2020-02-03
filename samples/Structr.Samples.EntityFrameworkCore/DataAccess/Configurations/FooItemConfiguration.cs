using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Structr.Samples.EntityFrameworkCore.Domain.FooAggregate;

namespace Structr.Samples.EntityFrameworkCore.DataAccess.Configurations
{
    class FooItemConfiguration : IEntityTypeConfiguration<FooItem>
    {
        public void Configure(EntityTypeBuilder<FooItem> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(100);

            builder.ToTable("FooItems");
        }
    }
}
