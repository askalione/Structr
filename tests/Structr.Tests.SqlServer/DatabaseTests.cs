using Structr.SqlServer;
using Xunit;

namespace Structr.Tests.SqlServer
{
    public class DatabaseTests
    {
        //public const string ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=tests_structr_sqlserver;Trusted_Connection=True;Encrypt=false;";
        public const string ConnectionString = @"Data Source=.\SQLEXPRESS;Database=tests_structr_sqlserver; Trusted_Connection=True;Encrypt=false;";
        [Fact]
        public void EnsureDeleted()
        {

        }

        [Fact]
        public void EnsureCreated()
        {
            // Arrange
            Database.EnsureCreated(ConnectionString);

            // Act

            // Assert
        }
    }
}