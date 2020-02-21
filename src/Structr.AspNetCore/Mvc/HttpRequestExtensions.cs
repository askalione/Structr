using Microsoft.AspNetCore.Http;

namespace Structr.AspNetCore.Mvc
{
    public static class HttpRequestExtensions
    {
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            return request.Headers["x-requested-with"] == "XMLHttpRequest";
        }
    }
}
