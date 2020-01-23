using Structr.Samples.EntityFrameworkCore.Domain.FooAggregate;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
