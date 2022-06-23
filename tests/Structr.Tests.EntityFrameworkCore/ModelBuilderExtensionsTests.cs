using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using Structr.Domain;
using Structr.EntityFrameworkCore;

namespace Structr.Tests.EntityFrameworkCore
{
    public class ModelBuilderExtensionsTests
    {
        private class Foo : Entity<Foo, int>
        {
            public Boo Boo { get; set; } = default!;
        }

        private class Boo : ValueObject<Boo>
        {
            public string Name { get; set; } = default!;
        }

        private class FooConfiguration : IEntityTypeConfiguration<Foo>
        {
            public void Configure(EntityTypeBuilder<Foo> builder)
            {
                builder.OwnsOne(x => x.Boo);
            }
        }

        private class TestDbContext : DbContext
        {
            public DbSet<Foo> Foos { get; private set; } = default!;

            public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }

            protected override void OnModelCreating(ModelBuilder builder)
            {
                builder.ApplyConfigurationsFromAssembly(GetType().Assembly);

                builder.ApplyEntityConfiguration();
                builder.ApplyValueObjectConfiguration(options =>
                {
                    options.Configure = (entityType, builder) =>
                    {
                        foreach (var property in entityType.GetProperties())
                        {
                            property.SetColumnName("prefix_" + property.Name);
                        }
                    };
                });
                builder.ApplyAuditableConfiguration();
            }
        }

        [Fact]
        public void ApplyEntityConfiguration()
        {
            // Arrange
            var serviceProvider = new ServiceCollection()
                .AddDbContext<TestDbContext>(options =>
                {
                    options.UseInMemoryDatabase(nameof(TestDbContext));
                })
                .BuildServiceProvider();

            // Act
            TestDbContext context = serviceProvider.GetRequiredService<TestDbContext>();

            // Assert
            var primaryKeyName = context.Foos.EntityType.FindPrimaryKey()!.Properties
                .Select(x => x.Name).Single();
            primaryKeyName.Should().Be("Id");
        }

        [Fact]
        public void ApplyValueObjectConfiguration()
        {
            // Arrange
            var serviceProvider = new ServiceCollection()
                .AddDbContext<TestDbContext>(options =>
                {
                    options.UseInMemoryDatabase(nameof(TestDbContext));
                })
                .BuildServiceProvider();

            // Act
            TestDbContext context = serviceProvider.GetRequiredService<TestDbContext>();

            // Assert
            foreach (var navigation in context.Foos.EntityType.GetNavigations())
            {
                var entityType = navigation.TargetEntityType;
                var schema = entityType.GetSchema();
                var tableName = entityType.GetTableName();
                var storeObjectIdentifier = StoreObjectIdentifier.Table(tableName!, schema);

                foreach (var property in entityType.GetProperties())
                {
                    var columnName = property.GetColumnName(storeObjectIdentifier);
                    columnName.Should().StartWith("prefix_");
                }
            }
        }

        [Fact]
        public void ApplyAuditableConfiguration()
        {
            // TODO
        }

        [Fact]
        public void GetEntityTypes()
        {
            // TODO
        }
    }
}
