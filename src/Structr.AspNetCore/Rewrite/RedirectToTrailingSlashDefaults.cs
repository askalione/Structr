namespace Structr.AspNetCore.Rewrite
{
    public static class RedirectToTrailingSlashDefaults
    {
        public const string MatchPattern = "^(((.*/)|(/?))[^/.]+(?!/$))$";
    }
}
