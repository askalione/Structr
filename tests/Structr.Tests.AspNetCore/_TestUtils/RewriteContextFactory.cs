using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;

namespace Structr.Tests.AspNetCore._TestUtils
{
    internal static class RewriteContextFactory
    {
        public static RewriteContext Create(string method, string path)
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Method = method;
            httpContext.Request.Scheme = "http";
            httpContext.Request.Host = new HostString("localhost:8080");
            httpContext.Request.Path = path;
            httpContext.Request.QueryString = new QueryString("?id=1");

            var rewriteContext = new RewriteContext { HttpContext = httpContext };

            return rewriteContext;
        }
    }
}
