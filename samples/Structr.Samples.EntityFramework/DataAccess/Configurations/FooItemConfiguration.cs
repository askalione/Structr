using Structr.Samples.EntityFrameworkCore.Domain.FooAggregate;
using System.Data.Entity.ModelConfiguration;

namespace Structr.Samples.EntityFramework.DataAccess.Configurations
{
    class FooItemConfiguration : EntityTypeConfiguration<FooItem>
    {
        public FooItemConfiguration()
        {
            Property(x => x.Name).HasMaxLength(100);

            ToTable("FooItems");
        }
    }
}
