using Structr.Samples.EntityFrameworkCore.Domain.FooAggregate;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
