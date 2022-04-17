using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Net.Http.Headers;
using System;
using System.Text.RegularExpressions;

namespace Structr.AspNetCore.Rewrite
{
    public class RedirectToTrailingSlashRule : IRule
    {
        private readonly int _statusCode;
        private readonly Func<HttpRequest, bool> _filter;

        public RedirectToTrailingSlashRule(Func<HttpRequest, bool> filter, int statusCode)
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

            var shouldUseTrailingSlash = Regex.IsMatch(request.Path.Value, RedirectToTrailingSlashDefaults.MatchPattern);

            if (shouldUseTrailingSlash == false)
            {
                context.Result = RuleResult.ContinueRules;
                return;
            }

            var url = UriHelper.BuildAbsolute(request.Scheme,
                new HostString(request.Host.Value),
                request.PathBase.Value,
                request.Path.Value + "/",
                request.QueryString);

            var response = context.HttpContext.Response;
            response.StatusCode = _statusCode;
            response.Headers[HeaderNames.Location] = url;
            context.Result = RuleResult.EndResponse;
        }
    }
}
