using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Net.Http.Headers;
using System;
using System.Linq;

namespace Structr.AspNetCore.Rewrite
{
    /// <summary>
    /// Rule performing redirect for GET requests to lower case url if case any upper characters are present.
    /// </summary>
    /// <remarks>Example: <c>http://localhost:5001/Home/Index => http://localhost:5001/home/index</c></remarks>
    public class RedirectToLowercaseRule : IRule
    {
        private readonly int _statusCode;
        private readonly Func<HttpRequest, bool> _filter;

        /// <summary>
        /// Creates an instance of <see cref="RedirectToLowercaseRule"/>
        /// </summary>
        /// <param name="filter">Function that determines if rule should be applied.</param>
        /// <param name="statusCode">Status code to redirect with.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="filter"/> is null.</exception>
        public RedirectToLowercaseRule(Func<HttpRequest, bool> filter, int statusCode)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            _statusCode = statusCode;
            _filter = filter;
        }

        public void ApplyRule(RewriteContext context)
        {
            var request = context.HttpContext.Request;

            var shouldRedirect = _filter(request);

            if (shouldRedirect == false)
            {
                context.Result = RuleResult.ContinueRules;
                return;
            }

            if (request.Method.ToUpperInvariant() != "GET")
            {
                context.Result = RuleResult.ContinueRules;
                return;
            }

            var shouldUseLowercase = request.Scheme.Any(char.IsUpper)
                || request.Host.Value.Any(char.IsUpper)
                || request.PathBase.Value.Any(char.IsUpper)
                || request.Path.Value.Any(char.IsUpper);

            if (shouldUseLowercase == false)
            {
                context.Result = RuleResult.ContinueRules;
                return;
            }

            var url = UriHelper.BuildAbsolute(request.Scheme.ToLowerInvariant(),
                new HostString(request.Host.Value.ToLowerInvariant()),
                request.PathBase.Value.ToLowerInvariant(),
                request.Path.Value.ToLowerInvariant(),
                request.QueryString);

            var response = context.HttpContext.Response;
            response.StatusCode = _statusCode;
            response.Headers[HeaderNames.Location] = url;
            context.Result = RuleResult.EndResponse;
        }
    }
}