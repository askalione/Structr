namespace Structr.AspNetCore.Internal
{
    public static class RedirectToTrailingSlashConstants
    {
        public const string MatchPattern = "^(((.*/)|(/?))[^/.]+(?!/$))$";
    }
}
