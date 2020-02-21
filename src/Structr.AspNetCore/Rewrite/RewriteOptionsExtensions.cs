using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using System;

namespace Structr.AspNetCore.Rewrite
{
    public static class RewriteOptionsExtensions
    {
        public static RewriteOptions AddRedirectToLowercase(this RewriteOptions options)
            => AddRedirectToLowercase(options, StatusCodes.Status307TemporaryRedirect);

        public static RewriteOptions AddRedirectToLowercasePermanent(this RewriteOptions options)
            => AddRedirectToLowercase(options, StatusCodes.Status308PermanentRedirect);

        public static RewriteOptions AddRedirectToLowercase(this RewriteOptions options, int statusCode)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            options.Add(new RedirectToLowercaseRule(statusCode));
            return options;
        }

        public static RewriteOptions AddRedirectToTrailingSlash(this RewriteOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            return options.AddRedirect(@"^(((.*/)|(/?))[^/.]+(?!/$))$", "$1/", StatusCodes.Status301MovedPermanently);
        }
    }
}
