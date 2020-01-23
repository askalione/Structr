using Structr.Abstractions;
using Structr.Abstractions.Providers;
using Structr.EntityFramework;
using Structr.Samples.EntityFrameworkCore.Domain.FooAggregate;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Samples.EntityFramework.DataAccess
{
    public class DataContext : DbContext
    {
        public DbSet<Foo> Foos { get; set; }

        private readonly ITimestampProvider _timestampProvider;
        private readonly IPrincipal _principal;

        public DataContext(string nameOrConnectionString, ITimestampProvider timestampProvider, IPrincipal principal)
            : base(nameOrConnectionString)
        {
            Ensure.NotNull(timestampProvider, nameof(timestampProvider));
            Ensure.NotNull(principal, nameof(principal));

            _timestampProvider = timestampProvider;
            _principal = principal;

            Database.Log = log => System.Diagnostics.Debug.WriteLine(log);
        }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            builder.ApplyEntityConfiguration();
            builder.ApplyValueObjectConfiguration();
            builder.ApplyAuditableConfiguration();

            builder.Configurations.AddFromAssembly(GetType().Assembly);
        }

        public override int SaveChanges()
        {
            this.Audit(_timestampProvider, _principal);
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            this.Audit(_timestampProvider, _principal);
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
