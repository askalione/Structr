using Effort.Provider;
using Structr.Domain;
using Structr.EntityFramework;
using Structr.Tests.EntityFramework.TestUtils;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Reflection;

namespace Structr.Tests.EntityFramework
{
    public class DbModelBuilderExtensionsTests
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

        private class TestDbContext : DbContext
        {
            public DbSet<Foo> Foos { get; set; } = default!;
            public DbSet<Bar> Bars { get; set; } = default!;

            public TestDbContext(DbConnection dbConnection) : base(dbConnection, true) { }

            protected override void OnModelCreating(DbModelBuilder builder)
            {
                builder.Configurations.AddFromAssembly(GetType().Assembly);
                builder.ApplyEntityConfiguration();
                builder.ApplyValueObjectConfiguration(options =>
                {
                    options.Configure = (typeConfiguration) =>
                    {
                        foreach (var property in typeConfiguration.ClrType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                        {
                            typeConfiguration.Property(property).HasColumnName("prefix_" + property.Name);
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
            EffortConnection connection = Effort.DbConnectionFactory.CreateTransient();

            // Act
            var context = new TestDbContext(connection);

            // Assert
            EntityType fooType = context.GetEntityType(typeof(Foo));
            var primaryKeyName = fooType.KeyProperties.Select(p => p.Name).Single();
            primaryKeyName.Should().Be("Id");
        }

        [Fact]
        public void ApplyValueObjectConfiguration()
        {
            // Arrange
            EffortConnection connection = Effort.DbConnectionFactory.CreateTransient();

            // Act
            var context = new TestDbContext(connection);

            // Assert
            EntityType fooType = context.GetEntityType(typeof(Foo));
            // TODO: check that all properties of Boo start with "prefix_"
        }

        [Fact]
        public void ApplyAuditableConfiguration()
        {
            // Arrange
            EffortConnection connection = Effort.DbConnectionFactory.CreateTransient();

            // Act
            var context = new TestDbContext(connection);

            // Assert
            EntityType barType = context.GetEntityType(typeof(Bar));
            EdmProperty dateCreatedProp = barType.Properties.First(x => x.Name == "DateCreated");
            EdmProperty createdByProp = barType.Properties.First(x => x.Name == "CreatedBy");
            EdmProperty dateModifiedProp = barType.Properties.First(x => x.Name == "DateModified");
            EdmProperty modifiedByProp = barType.Properties.First(x => x.Name == "ModifiedBy");
            EdmProperty dateDeletedProp = barType.Properties.First(x => x.Name == "DateDeleted");
            EdmProperty deletedByProp = barType.Properties.First(x => x.Name == "DeletedBy");

            dateCreatedProp.Should().NotBeNull();
            dateCreatedProp.Nullable.Should().BeFalse();
            createdByProp.Should().NotBeNull();
            // TODO: Nullable is true, but should be false
            //createdByProp.Nullable.Should().Be(!_signedColumnIsRequired);
            // TODO: MaxLength is null
            //createdByProp.MaxLength.Should().Be(_signedColumnMaxLength);

            dateModifiedProp.Should().NotBeNull();
            dateModifiedProp.Nullable.Should().BeFalse();
            modifiedByProp.Should().NotBeNull();
            // TODO: Nullable is true, but should be false
            //modifiedByProp.Nullable.Should().Be(!_signedColumnIsRequired);
            // TODO: MaxLength is null
            //modifiedByProp.MaxLength.Should().Be(_signedColumnMaxLength);

            dateDeletedProp.Should().NotBeNull();
            dateDeletedProp.Nullable.Should().BeTrue();
            deletedByProp.Should().NotBeNull();
            deletedByProp.Nullable.Should().BeTrue();
            // TODO: MaxLength is null
            //deletedByProp.MaxLength.Should().Be(_signedColumnMaxLength);
        }
    }
}
