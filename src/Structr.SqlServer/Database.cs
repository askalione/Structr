using System;
using System.Data.SqlClient;
using System.IO;

namespace Structr.SqlServer
{
    /// <summary>
    /// Database MS SQL Server.
    /// </summary>
    public static class Database
    {
        /// <summary>
        /// Method that guarantees creation of a database for MS SQL Server.
        /// </summary>
        /// <param name="connectionString">Connection string for MS SQL Server.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="connectionString"/> is null.</exception>
        public static void EnsureCreated(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(connectionString);
            }

            var builder = new SqlConnectionStringBuilder(connectionString);
            string database = builder.GetDatabase();
            if (builder.HasAttachDBFilename())
            {
                database = builder.AttachDBFilename;
            }

            string statement = $@"IF NOT EXISTS(SELECT * FROM sys.databases WHERE NAME='{database}')
                                   CREATE DATABASE [{database}]";

            if (builder.HasAttachDBFilename())
            {
                string databaseFilename = builder.AttachDBFilename;
                string logFilename = builder.GetAttachDBLogFilename();
                string log = Path.GetFileNameWithoutExtension(logFilename);

                statement += $" ON PRIMARY(NAME = '{database}', FILENAME = '{databaseFilename}') LOG ON(NAME = '{log}', FILENAME = '{logFilename}')";
            }

            ExecuteStatement(builder, statement);
        }

        /// <summary>
        /// Method that guarantees deletion of a database for MS SQL Server.
        /// </summary>
        /// <param name="connectionString">Connection string for MS SQL Server.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="connectionString"/> is null.</exception>
        public static void EnsureDeleted(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(connectionString);
            }               

            var builder = new SqlConnectionStringBuilder(connectionString);
            string database = builder.GetDatabase();
            if (builder.HasAttachDBFilename())
            {
                database = builder.AttachDBFilename;
            }

            ExecuteStatement(builder,
                $@"IF EXISTS(SELECT * FROM sys.databases WHERE NAME='{database}')                       
                       DROP DATABASE [{database}]");
            
            if (builder.HasAttachDBFilename())
            {
                string databaseFilename = builder.AttachDBFilename;
                string logFilename = builder.GetAttachDBLogFilename();

                if (File.Exists(logFilename))
                {
                    File.Delete(logFilename);
                }
                if (File.Exists(databaseFilename))
                {
                    File.Delete(databaseFilename);
                }                   
            }
        }

        private static void ExecuteStatement(SqlConnectionStringBuilder builder, string statement)
        {
            builder.InitialCatalog = "";
            string connectionString = builder.HasAttachDBFilename() ? $"Data Source={builder.DataSource}" : builder.ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = statement;
                    command.ExecuteNonQuery();                  
                }
            }
        }

        private static bool HasAttachDBFilename(this SqlConnectionStringBuilder builder)
            => string.IsNullOrWhiteSpace(builder.AttachDBFilename) == false;

        private static string GetDatabase(this SqlConnectionStringBuilder builder)
        {
            return string.IsNullOrWhiteSpace(builder.InitialCatalog) == false
                ? builder.InitialCatalog
                : (builder.HasAttachDBFilename() ? Path.GetFileNameWithoutExtension(builder.AttachDBFilename) : "");
        }

        private static string GetAttachDBLogFilename(this SqlConnectionStringBuilder builder)
        {
            if (builder.HasAttachDBFilename() == false)
            {
                return null;
            }               

            var databaseFilename = builder.AttachDBFilename;
            var log = builder.GetDatabase() + "_log";
            return Path.Combine(Path.GetDirectoryName(databaseFilename), log + ".ldf");
        }
    }
}
