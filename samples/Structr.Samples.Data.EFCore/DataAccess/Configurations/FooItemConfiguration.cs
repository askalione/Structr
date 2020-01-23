using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Structr.Samples.Data.EFCore.Domain.FooAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace Structr.Samples.Data.EFCore.DataAccess.Configurations
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
