using FluentAssertions;
using Structr.SqlServer;
using System.Data.SqlClient;
using Xunit;

namespace Structr.Tests.SqlServer
{
    public class DatabaseTests
    {
        public const string ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=tests_structr_sqlserver;Trusted_Connection=True;Encrypt=false;";
        public const string ConnectionString2 = @"Data Source=.\SQLEXPRESS;Initial Catalog=tests_sqlserver;Trusted_Connection=True;Encrypt=false;";
        public static string dbPath = TestDataPath.Combine("tests_structr_sqlserver.mdf").Replace("/", @"\");
        //public static string dbPath = @"F:\Dev\Structr\tests\Structr.Tests.SqlServer\TestData\tests_structr_sqlserver.mdf";
        //public static string dbPath2 = @"F:\Dev\Structr\tests\Structr.Tests.SqlServer\TestData\tests_structr_sqlserver.mdf";
        public static string dbPath2 = TestDataPath.Combine("tests_structr_sqlserver.mdf").Replace("/", @"\");
        public static string ConnectionStringMdf2 = @$"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dbPath};Integrated Security=True;";     
        public static string ConnectionStringMdf = @$"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dbPath2};Integrated Security=True;";
        //public static string ConnectionStringMdf2 = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\USERS\СЕРГЕЙ\DOCUMENTS\tests_sqlserver_new.MDF;Integrated Security=True;Encrypt=false;";
        //public const string ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=tests_sqlserver; Trusted_Connection=True;Encrypt=false;"; tests_sqlserver_new
        [Fact]
        public void EnsureDeleted()
        {
            var builder = new SqlConnectionStringBuilder(ConnectionString2);           

            // Arrange
            Database.EnsureDeleted(ConnectionString2);
           // Database.EnsureDeleted(ConnectionStringMdf);
           

            var statement = $@"SELECT * FROM sys.databases WHERE NAME='tests_sqlserver'";


            int rows = -1;
            using (var connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = statement;
                    rows = command.ExecuteNonQuery();
                    command.Dispose();
                }

                connection.Close();
            }

            // Act

            // Assert
            rows.Should().Be(-1);
        }

        [Fact]
        public void EnsureDeletedMdf()
        {
            // Arrange          
            Database.EnsureDeleted(ConnectionStringMdf);

            var builder = new SqlConnectionStringBuilder(ConnectionStringMdf);                    

            int rows = -1;
            using (var connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand($@"SELECT * FROM sys.databases WHERE NAME='{builder.AttachDBFilename}'", connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        rows++;
                    }

                    reader.Close();
                }
            }

            // Act

            // Assert
            rows.Should().Be(-1);
        }

        [Fact]
        public void EnsureCreated()
        {
            // Arrange
            Database.EnsureCreated(ConnectionString);            

            var builder = new SqlConnectionStringBuilder(ConnectionString);          

            int rows = -1;
            using (var connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand($@"SELECT * FROM sys.databases WHERE NAME='{builder.InitialCatalog}'", connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                       rows++;
                    }

                    reader.Close();
                }

                connection.Close();
                connection.Dispose();
            }

            // Act

            // Assert
            rows.Should().Be(0);
        }

        [Fact]
        public void EnsureCreatedMdf()
        {
            // Arrange
            //Database.EnsureCreated(ConnectionString);
            Database.EnsureCreated(ConnectionStringMdf2);

            var builder = new SqlConnectionStringBuilder(ConnectionStringMdf2);
            var databaseFilename = builder.AttachDBFilename;          
           

            int rows = -1;
            using (var connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand($@"SELECT * FROM sys.databases WHERE NAME='{databaseFilename}'", connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        rows++;
                    }

                    reader.Close();
                }
            }

            //Act

            //Assert
            rows.Should().NotBe(-1);
        }
    }
}