using FluentAssertions;
using Structr.SqlServer;
using System.Data.SqlClient;
using Xunit;

namespace Structr.Tests.SqlServer
{
    public class DatabaseTests
    {
        public const string ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=tests_structr_sqlserver;Trusted_Connection=True;Encrypt=false;";
        //public static string dbPath = TestDataPath.Combine("tests_structr_sqlserver.mdf");
        //public static string ConnectionStringMdf = @$"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dbPath};Integrated Security=True;Encrypt=false;"; // tests_structr_sqlserver
        public static string ConnectionStringMdf = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\USERS\СЕРГЕЙ\DOCUMENTS\TESTS_STRUCTR_SQLSERVER.MDF;Integrated Security=True;Encrypt=false;";
        //public const string ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=tests_sqlserver; Trusted_Connection=True;Encrypt=false;";
        [Fact]
        public void EnsureDeleted()
        {
            // Arrange
            Database.EnsureDeleted(ConnectionString);
           // Database.EnsureDeleted(ConnectionStringMdf);

            var builder = new SqlConnectionStringBuilder(ConnectionString);

            var statement = $@"SELECT * FROM sys.databases WHERE NAME='tests_structr_sqlserver'";


            int rows = 0;
            using (var connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = statement;
                    rows = command.ExecuteNonQuery();
                }

                connection.Close();
            }

            // Act

            // Assert
            rows.Should().Be(0);
        }

        [Fact]
        public void EnsureDeletedMdf()
        {
            // Arrange
           // Database.EnsureDeleted(ConnectionString);
            Database.EnsureDeleted(ConnectionStringMdf);

            var builder = new SqlConnectionStringBuilder(ConnectionStringMdf);

            var statement = $@"SELECT * FROM sys.databases WHERE NAME='C:\USERS\СЕРГЕЙ\DOCUMENTS\TESTS_STRUCTR_SQLSERVER.MDF'";


            int rows = 0;
            using (var connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = statement;
                    rows = command.ExecuteNonQuery();
                }

                connection.Close();
            }

            // Act

            // Assert
            rows.Should().Be(0);
        }

        [Fact]
        public void EnsureCreated()
        {
            // Arrange
            Database.EnsureCreated(ConnectionString);
            //Database.EnsureCreated(ConnectionStringMdf);

            var builder = new SqlConnectionStringBuilder(ConnectionString);

            var statement = $@"SELECT * FROM sys.databases WHERE NAME='tests_structr_sqlserver'";


            int rows = 0;
            using (var connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = statement;
                    rows = command.ExecuteNonQuery();
                }

                connection.Close();
            }

            // Act

            // Assert
            rows.Should().NotBe(0);
        }

        [Fact]
        public void EnsureCreatedMdf()
        {
            // Arrange
            //Database.EnsureCreated(ConnectionString);
            Database.EnsureCreated(ConnectionStringMdf);

            var builder = new SqlConnectionStringBuilder(ConnectionStringMdf);

            var statement = $@"SELECT * FROM sys.databases WHERE NAME='C:\USERS\СЕРГЕЙ\DOCUMENTS\TESTS_STRUCTR_SQLSERVER.MDF'";


            int rows = 0;
            using (var connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = statement;
                    rows = command.ExecuteNonQuery();
                }

                connection.Close();
            }

            // Act

            // Assert
            rows.Should().NotBe(0);
        }
    }
}