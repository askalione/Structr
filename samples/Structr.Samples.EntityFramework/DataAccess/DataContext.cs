using Structr.Abstractions.Providers;
using Structr.EntityFramework;
using Structr.Samples.EntityFrameworkCore.Domain.FooAggregate;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
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
            _timestampProvider = timestampProvider;
            _principal = principal;

            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            Database.Log = log => System.Diagnostics.Debug.WriteLine(log);
        }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            builder.ApplyEntityConfiguration();
            builder.ApplyValueObjectConfiguration(options =>
            {
                options.Configure = (typeConfiguration) =>
                {
                    foreach (var property in typeConfiguration.ClrType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    {
                        typeConfiguration.Property(property).HasColumnName(property.Name);
                    }
                };
            });
            builder.ApplyAuditableConfiguration();

            builder.Conventions.Remove<PluralizingTableNameConvention>();
            builder.Conventions.Remove<PluralizingEntitySetNameConvention>();

            builder.Properties<string>().Configure(x => x.IsUnicode(true));

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
