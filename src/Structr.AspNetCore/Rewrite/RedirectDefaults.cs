namespace Structr.AspNetCore.Rewrite
{
    /// <summary>
    /// Constants related to redirecting.
    /// </summary>
    public static class RedirectDefaults
    {
        /// <summary>
        /// Match pattern for using trailing slash.
        /// </summary>
        public const string TrailingSlashMatchPattern = "^(((.*/)|(/?))[^/.]+(?!/$))$";
    }
}
