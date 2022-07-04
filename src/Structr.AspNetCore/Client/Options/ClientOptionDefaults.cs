namespace Structr.AspNetCore.Client.Options
{
    /// <summary>
    /// Defines constants related to Client options.
    /// </summary>
    public static class ClientOptionDefaults
    {
        /// <summary>
        /// Delimiter between parts of key identifying Client options stored in <see cref="Microsoft.AspNetCore.Http.HttpContext.Items"/>.
        /// </summary>
        public static string Delimiter = ".";
    }
}
