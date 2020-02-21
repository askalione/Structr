using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Net.Http.Headers;
using System.Linq;

namespace Structr.AspNetCore.Rewrite
{
    public class RedirectToLowercaseRule : IRule
    {
        private readonly int _statusCode;

        public RedirectToLowercaseRule(int statusCode)
        {
            _statusCode = statusCode;
        }

        public void ApplyRule(RewriteContext context)
        {
            var request = context.HttpContext.Request;

            if (!request.Scheme.Any(char.IsUpper)
                && !request.Host.Value.Any(char.IsUpper)
                && !request.PathBase.Value.Any(char.IsUpper)
                && !request.Path.Value.Any(char.IsUpper))
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
            context.Logger.RedirectedToLowercase();
        }
    }
}