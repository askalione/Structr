namespace Structr.Abstractions.Providers.SequentialGuid
{
    /// <summary>
    /// A type of the sequential GUID.
    /// </summary>
    public enum SequentialGuidType
    {
        /// <summary>
        /// The first six bytes are in sequential order, and the remainder is random.
        /// Inserting these values into a database that stores GUIDs as strings (such as MySQL)
        /// should provide a performance gain over non-sequential values.
        /// This type should be used with MySQL or PostgreSQL database.
        /// </summary>
        String,

        /// <summary>
        /// The first two blocks are "jumbled" due to having all their bytes reversed (this is due to the endianness issue discussed earlier).
        /// If we were to insert these values into a text field (like they would be under MySQL or PostgreSQL), the performance would not be ideal.
        /// This type should be used with Oracle database.
        /// </summary>
        Binary,

        /// <summary>
        /// The last six bytes are in sequential order, and the rest is random.
        /// This type should be used with MS SQL Server database.
        /// </summary>
        Ending,
    }
}
