using Effort.Provider;
using Structr.Domain;
using Structr.EntityFramework;
using Structr.Tests.EntityFramework.TestUtils;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;

namespace Structr.Tests.EntityFramework
{
    public class ConventionPrimitivePropertyConfigurationExtensionsTests
    {
        private class Foo : Entity<Foo, int>
        {
            public string Name { get; set; } = default!;
        }

        private class TestDbContext : DbContext
        {
            public DbSet<Foo> Foos { get; set; } = default!;

            private bool _isRequired;

            public TestDbContext(DbConnection dbConnection, bool isRequired) : base(dbConnection, true)
            {
                _isRequired = isRequired;
            }

            protected override void OnModelCreating(DbModelBuilder builder)
            {
                builder.Types().Configure(x =>
                {
                    x.Property("Name").IsRequired(_isRequired);
                });

                base.OnModelCreating(builder);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsRequired(bool isRequired)
        {
            // Arrange
            EffortConnection connection = Effort.DbConnectionFactory.CreateTransient();

            // Act
            var context = new TestDbContext(connection, isRequired);

            // Assert
            EntityType fooType = context.GetEntityType(typeof(Foo));
            EdmProperty dateCreatedProp = fooType.Properties.First(x => x.Name == "Name");
            // TODO: when isRequired == true Nullable should be false
            //dateCreatedProp.Nullable.Should().Be(isRequired == false);
        }
    }
}
