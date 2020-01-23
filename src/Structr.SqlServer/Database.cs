using System;
using System.Data.SqlClient;
using System.IO;

namespace Structr.SqlServer
{
    public static class Database
    {
        public static void EnsureDeleted(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(connectionString);

            var builder = new SqlConnectionStringBuilder(connectionString);
            var database = builder.GetDatabase();

            ExecuteStatement(builder,
                $@"IF EXISTS(SELECT * FROM sys.databases WHERE NAME='{database}')
                        DROP DATABASE [{database}]");
        }

        public static void EnsureCreated(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(connectionString);

            var builder = new SqlConnectionStringBuilder(connectionString);
            var database = builder.GetDatabase();

            var statement = $@"IF NOT EXISTS(SELECT * FROM sys.databases WHERE NAME='{database}')
                        CREATE DATABASE [{database}]";

            if (!string.IsNullOrEmpty(builder.AttachDBFilename))
            {
                var databaseFilename = builder.AttachDBFilename;
                var log = database + "_log";
                var logFilename = Path.Combine(Path.GetDirectoryName(databaseFilename), log + ".ldf");

                statement += $" ON PRIMARY(NAME = '{database}', FILENAME = '{databaseFilename}') LOG ON(NAME = '{log}', FILENAME = '{logFilename}')";
            }

            ExecuteStatement(builder, statement);
        }

        private static void ExecuteStatement(SqlConnectionStringBuilder builder, string statement)
        {
            using (var connection = new SqlConnection($"Data Source={builder.DataSource}"))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = statement;
                    command.ExecuteNonQuery();
                }
            }
        }

        private static string GetDatabase(this SqlConnectionStringBuilder builder)
        {
            return !string.IsNullOrWhiteSpace(builder.InitialCatalog)
                ? builder.InitialCatalog
                : (!string.IsNullOrEmpty(builder.AttachDBFilename) ? Path.GetFileNameWithoutExtension(builder.AttachDBFilename) : "");
        }
    }
}
