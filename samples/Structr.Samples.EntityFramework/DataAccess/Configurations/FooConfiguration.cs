using Structr.Samples.EntityFrameworkCore.Domain.FooAggregate;
using System.Data.Entity.ModelConfiguration;

namespace Structr.Samples.EntityFramework.DataAccess.Configurations
{
    class FooConfiguration : EntityTypeConfiguration<Foo>
    {
        public FooConfiguration()
        {
            HasMany(x => x.Items)
                .WithRequired()
                .HasForeignKey(x => x.FooId)
                .WillCascadeOnDelete(true);

            ToTable("Foos");
        }
    }
}
