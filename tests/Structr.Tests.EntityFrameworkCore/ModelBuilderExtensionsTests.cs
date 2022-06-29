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
        private const bool _signedColumnIsRequired = true;
        private const int _signedColumnMaxLength = 100;

        private class Foo : Entity<Foo, int>
        {
            public Boo Boo { get; set; } = default!;
        }

        private class Boo : ValueObject<Boo>
        {
            public string Name { get; set; } = default!;
        }

        private class Bar : SignedAuditableEntity<Bar, int>, ISignedSoftDeletable
        {
            public string? DeletedBy { get; set; }

            public DateTime? DateDeleted { get; set; }
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
            public DbSet<Bar> Bars { get; private set; } = default!;

            public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }

            protected override void OnModelCreating(ModelBuilder builder)
            {
                builder.ApplyConfigurationsFromAssembly(GetType().Assembly);

                builder.ApplyEntityConfiguration();
                builder.ApplyValueObjectConfiguration(options =>
                {
                    options.Configure = (entityType, navigationName, builder) =>
                    {
                        foreach (var property in entityType.GetProperties())
                        {
                            property.SetColumnName("prefix_" + property.Name);
                        }
                    };
                });
                builder.ApplyAuditableConfiguration(options =>
                {
                    options.SignedColumnIsRequired = _signedColumnIsRequired;
                    options.SignedColumnMaxLength = _signedColumnMaxLength;
                });
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
            IEntityType barType = context.Bars.EntityType;
            IProperty? dateCreatedProp = barType.FindProperty("DateCreated");
            IProperty? createdByProp = barType.FindProperty("CreatedBy");
            IProperty? dateModifiedProp = barType.FindProperty("DateModified");
            IProperty? modifiedByProp = barType.FindProperty("ModifiedBy");
            IProperty? dateDeletedProp = barType.FindProperty("DateDeleted");
            IProperty? deletedByProp = barType.FindProperty("DeletedBy");

            dateCreatedProp.Should().NotBeNull();
            dateCreatedProp!.IsNullable.Should().BeFalse();
            createdByProp.Should().NotBeNull();
            createdByProp!.IsNullable.Should().Be(!_signedColumnIsRequired);
            createdByProp!.GetMaxLength().Should().Be(_signedColumnMaxLength);

            dateModifiedProp.Should().NotBeNull();
            dateModifiedProp!.IsNullable.Should().BeFalse();
            modifiedByProp.Should().NotBeNull();
            modifiedByProp!.IsNullable.Should().Be(!_signedColumnIsRequired);
            modifiedByProp!.GetMaxLength().Should().Be(_signedColumnMaxLength);

            dateDeletedProp.Should().NotBeNull();
            dateDeletedProp!.IsNullable.Should().BeTrue();
            deletedByProp.Should().NotBeNull();
            deletedByProp!.IsNullable.Should().BeTrue();
            deletedByProp!.GetMaxLength().Should().Be(_signedColumnMaxLength);
        }

        [Fact]
        public void GetEntityTypes_entities()
        {
            // Arrange
            var modelBuilder = new ModelBuilder();
            modelBuilder.Entity<Foo>().OwnsOne(x => x.Boo);
            modelBuilder.Entity<Bar>();

            // Act
            List<IMutableEntityType> entityTypes = modelBuilder.GetEntityTypes(typeof(Entity<>));

            // Assert
            entityTypes.Should().Satisfy(
                foo => foo.ClrType == typeof(Foo),
                bar => bar.ClrType == typeof(Bar)
            );
        }

        [Fact]
        public void GetEntityTypes_value_objects()
        {
            // Arrange
            var modelBuilder = new ModelBuilder();
            modelBuilder.Entity<Foo>().OwnsOne(x => x.Boo);
            modelBuilder.Entity<Bar>();

            // Act
            List<IMutableEntityType> entityTypes = modelBuilder.GetEntityTypes(typeof(ValueObject<>));

            // Assert
            entityTypes.Should().Satisfy(
                boo => boo.ClrType == typeof(Boo)
            );
        }

        [Theory]
        [InlineData(null)]
        public void GetEntityTypes_throws_when_modelBuilder_is_null(ModelBuilder modelBuilder)
        {
            // Act
            Action act = () => modelBuilder.GetEntityTypes(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void GetEntityTypes_throws_when_type_is_null()
        {
            // Arrange
            var modelBuilder = new ModelBuilder();

            // Act
            Action act = () => modelBuilder.GetEntityTypes(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}
