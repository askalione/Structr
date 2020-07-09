using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using System;

namespace Structr.AspNetCore.Rewrite
{
    public static class RewriteOptionsExtensions
    {
        public static RewriteOptions AddRedirectToLowercase(this RewriteOptions options)
            => AddRedirectToLowercase(options,
                (request) => true,
                StatusCodes.Status301MovedPermanently);

        public static RewriteOptions AddRedirectToLowercase(this RewriteOptions options, Func<HttpRequest, bool> filter, int statusCode)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            options.Add(new RedirectToLowercaseRule(filter, statusCode));
            return options;
        }

        public static RewriteOptions AddRedirectToTrailingSlash(this RewriteOptions options)
            => AddRedirectToTrailingSlash(options,
                (request) => true,
                StatusCodes.Status301MovedPermanently);

        public static RewriteOptions AddRedirectToTrailingSlash(this RewriteOptions options, Func<HttpRequest, bool> filter, int statusCode)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            options.Add(new RedirectToTrailingSlashRule(filter, statusCode));
            return options;
        }

        public static RewriteOptions AddRedirectToLowercaseTrailingSlash(this RewriteOptions options)
            => AddRedirectToLowercaseTrailingSlash(options,
                (request) => true,
                StatusCodes.Status301MovedPermanently);

        public static RewriteOptions AddRedirectToLowercaseTrailingSlash(this RewriteOptions options, Func<HttpRequest, bool> filter, int statusCode)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            options.Add(new RedirectToLowercaseTrailingSlashRule(filter, statusCode));
            return options;
        }
    }
}
