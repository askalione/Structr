using FluentAssertions;
using Structr.SqlServer;
using System.Data.SqlClient;
using System.IO;
using Xunit;

namespace Structr.Tests.SqlServer
{
    public class DatabaseTests
    {
        public const string ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=tests_structr_sqlserver;Integrated Security=True";
        public static string ConnectionStringMdf = @$"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={TestDataPath.Combine("tests_structr_sqlserver.mdf")};Integrated Security=True;";     
        
        [Fact(Skip = "The test is not complete. Needed clean up after executing.")]
        public void EnsureCreated()
        {
            // Act
            Database.EnsureCreated(ConnectionString);

            // Assert
            bool databaseExists = DatabaseExists(ConnectionString);
            databaseExists.Should().Be(true);
        }

        [Fact(Skip = "The test is not complete. Needed clean up after executing.")]
        public void EnsureDeleted()
        {
            // Arrange
            Database.EnsureCreated(ConnectionString);

            // Act
            Database.EnsureDeleted(ConnectionString);

            // Assert
            bool databaseExists = DatabaseExists(ConnectionString);
            databaseExists.Should().Be(false);
        }

        [Fact(Skip = "The test is not complete. Needed clean up after executing.")]
        public void EnsureCreatedMdf()
        {
            //Act
            Database.EnsureCreated(ConnectionStringMdf);

            //Assert
            bool databaseExists = DatabaseExists(ConnectionStringMdf);
            databaseExists.Should().Be(true);
        }

        [Fact(Skip = "The test is not complete. Needed clean up after executing.")]
        public void EnsureDeletedMdf()
        {
            // Arrange
            Database.EnsureCreated(ConnectionStringMdf);

            //Act
            Database.EnsureDeleted(ConnectionStringMdf);

            //Assert
            bool databaseExists = DatabaseExists(ConnectionStringMdf);
            databaseExists.Should().Be(false);
        }

        private bool DatabaseExists(string connectionString)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);
            bool hasAttachDBFilename = string.IsNullOrWhiteSpace(builder.AttachDBFilename) == false;
            string database = hasAttachDBFilename ? builder.AttachDBFilename : GetDatabase(builder);  
            builder.InitialCatalog = "";
            using (var connection = new SqlConnection(hasAttachDBFilename ? $"Data Source={builder.DataSource}" : builder.ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(
                    $@"SELECT * FROM sys.databases WHERE NAME='{database}'", connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    return reader.HasRows;
                }
            }
        }

        private string GetDatabase(SqlConnectionStringBuilder builder)
        {
            return string.IsNullOrWhiteSpace(builder.InitialCatalog) == false
                ? builder.InitialCatalog
                : (string.IsNullOrWhiteSpace(builder.AttachDBFilename) ? Path.GetFileNameWithoutExtension(builder.AttachDBFilename) : "");
        }
    }
}