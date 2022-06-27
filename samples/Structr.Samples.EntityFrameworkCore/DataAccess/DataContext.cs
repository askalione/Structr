using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Structr.Abstractions.Providers;
using Structr.Abstractions.Providers.Timestamp;
using Structr.EntityFrameworkCore;
using Structr.Samples.EntityFrameworkCore.Domain.FooAggregate;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Samples.EntityFrameworkCore.DataAccess
{
    public class DataContext : DbContext
    {
        public DbSet<Foo> Foos { get; private set; }

        private readonly ITimestampProvider _timestampProvider;
        private readonly IPrincipal _principal;

        public AuditTimestampProvider AuditTimestampProvider => _timestampProvider != null
            ? _timestampProvider.GetTimestamp
            : null;
        public AuditSignProvider AuditSignProvider => _principal != null
            ? () => _principal.Identity.Name
            : null;

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            _timestampProvider = options.GetService<ITimestampProvider>();
            _principal = options.GetService<IPrincipal>();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyEntityConfiguration();
            builder.ApplyValueObjectConfiguration(options =>
            {
                options.Configure = (entityType, builder) =>
                {
                    foreach (var property in entityType.GetProperties())
                    {
                        property.SetColumnName(property.Name);
                    }
                };
            });
            builder.ApplyAuditableConfiguration();
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.Audit(AuditTimestampProvider, AuditSignProvider);
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            this.Audit(AuditTimestampProvider, AuditSignProvider);
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
