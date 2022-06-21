using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using System;

namespace Structr.AspNetCore.Rewrite
{
    /// <summary>
    /// Defines extension methods on <see cref="RewriteOptions"/>.
    /// </summary>
    public static class RewriteOptionsExtensions
    {
        /// <summary>
        /// Define rule performing redirect with status 301 for GET requests to lower case url in
        /// case any upper characters are present.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        /// <remarks>Example: <c>http://localhost:5001/Home/Index => http://localhost:5001/home/index</c></remarks>
        public static RewriteOptions AddRedirectToLowercase(this RewriteOptions options)
            => AddRedirectToLowercase(options,
                (request) => true,
                StatusCodes.Status301MovedPermanently);

        /// <summary>
        /// Define rule performing redirect for GET requests to lower case url in
        /// case any upper characters are present.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="filter">Function that determines if rule should be applied.</param>
        /// <param name="statusCode">Status code to redirect with.</param>
        /// <inheritdoc cref="AddRedirectToLowercase(RewriteOptions)"/>
        public static RewriteOptions AddRedirectToLowercase(this RewriteOptions options, Func<HttpRequest, bool> filter, int statusCode)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            options.Add(new RedirectToLowercaseRule(filter, statusCode));
            return options;
        }

        /// <summary>
        /// Define rule performing redirect with status 301 for GET requests by adding a trailing slash.
        /// </summary>
        /// <remarks>Example: <c>http://localhost:5001/Home/Index?search=hello => http://localhost:5001/Home/Index/?search=hello</c></remarks>
        /// <inheritdoc cref="AddRedirectToLowercase(RewriteOptions)"/>
        public static RewriteOptions AddRedirectToTrailingSlash(this RewriteOptions options)
            => AddRedirectToTrailingSlash(options,
                (request) => true,
                StatusCodes.Status301MovedPermanently);

        /// <summary>
        /// Define rule performing redirect with status 301 for GET requests by adding a trailing slash.
        /// </summary>
        /// <inheritdoc cref="AddRedirectToTrailingSlash(RewriteOptions)"/>
        /// <inheritdoc cref="AddRedirectToTrailingSlash(RewriteOptions, Func{HttpRequest, bool}, int)"/>
        public static RewriteOptions AddRedirectToTrailingSlash(this RewriteOptions options, Func<HttpRequest, bool> filter, int statusCode)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            options.Add(new RedirectToTrailingSlashRule(filter, statusCode));
            return options;
        }

        /// <summary>
        /// Define rule performing redirect with status 301 for GET requests to lower case url in
        /// case any upper characters are present, while adding trailing slash.
        /// </summary>
        /// <remarks>Example: <c>http://localhost:5001/Home/Index?search=hello => http://localhost:5001/home/index/?search=hello</c></remarks>
        public static RewriteOptions AddRedirectToLowercaseTrailingSlash(this RewriteOptions options)
            => AddRedirectToLowercaseTrailingSlash(options,
                (request) => true,
                StatusCodes.Status301MovedPermanently);

        /// <summary>
        /// Define rule performing redirect for GET requests to lower case url in
        /// case any upper characters are present, while adding trailing slash.
        /// </summary>
        /// <inheritdoc cref="AddRedirectToLowercaseTrailingSlash(RewriteOptions)"/>
        /// <inheritdoc cref="AddRedirectToLowercaseTrailingSlash(RewriteOptions, Func{HttpRequest, bool}, int)"/>
        public static RewriteOptions AddRedirectToLowercaseTrailingSlash(this RewriteOptions options, Func<HttpRequest, bool> filter, int statusCode)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            options.Add(new RedirectToLowercaseTrailingSlashRule(filter, statusCode));
            return options;
        }
    }
}
