using Microsoft.EntityFrameworkCore;
using Structr.Data.EFCore;
using Structr.Samples.Data.EFCore.Domain.FooAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace Structr.Samples.Data.EFCore.DataAccess
{
    public class DataContext : DbContext
    {
        public DbSet<Foo> Foos { get; private set; }
        
        public DataContext(DbContextOptions options) : base(options)
        {
            this.UseAudit();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyEntityConfiguration();
            builder.ApplyValueObjectConfiguration();
            builder.ApplyAuditableConfiguration();
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
