using Microsoft.EntityFrameworkCore;
using Structr.EntityFrameworkCore;
using Structr.Samples.EntityFrameworkCore.WebApp.Domain;
using Structr.Samples.EntityFrameworkCore.WebApp.Domain.Issues;
using Structr.Samples.EntityFrameworkCore.WebApp.Domain.Projects;
using Structr.Samples.EntityFrameworkCore.WebApp.Domain.Users;

namespace Structr.Samples.EntityFrameworkCore.WebApp.DataAccess
{
    public class AppDbContext : DbContext
    {
        public DbSet<Issue> Issues { get; set; } = default!;
        public DbSet<Project> Projects { get; set; } = default!;
        public DbSet<User> Users { get; set; } = default!;

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            //builder.ApplyEntityConfiguration();
            builder.ApplyValueObjectConfiguration(options =>
            {
                options.Configure = (entityType, navigationName, builder) =>
                {
                    bool isMultilang = entityType.ClrType == typeof(Multilang);
                    Func<string, string> columnName = (propName) => isMultilang ? navigationName + propName : propName;

                    foreach (var property in entityType.GetProperties().Where(x => x.IsPrimaryKey() == false))
                    {
                        property.SetColumnName(columnName(property.Name));
                    }
                };
            });
            //builder.ApplyAuditableConfiguration();
        }
    }
}
