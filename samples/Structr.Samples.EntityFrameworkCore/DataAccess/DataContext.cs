using Microsoft.EntityFrameworkCore;
using Structr.Samples.EntityFrameworkCore.Domain.FooAggregate;
using Structr.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Samples.EntityFrameworkCore.DataAccess
{
    public class DataContext : DbContext
    {
        public DbSet<Foo> Foos { get; private set; }

        public DataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyEntityConfiguration();
            builder.ApplyValueObjectConfiguration();
            builder.ApplyAuditableConfiguration();
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.Audit();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            this.Audit();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
