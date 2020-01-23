using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Structr.Samples.Data.EFCore.Domain.FooAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace Structr.Samples.Data.EFCore.DataAccess.Configurations
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
