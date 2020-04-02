using Microsoft.AspNetCore.Http;
using Structr.AspNetCore.Internal;
using System;

namespace Structr.AspNetCore.Mvc
{
    public static class HttpRequestExtensions
    {
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return request.Headers["x-requested-with"] == "XMLHttpRequest";
        }

        public static string GetAbsoluteUri(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            UriBuilder uriBuilder = new UriBuilder();
            uriBuilder.Scheme = request.Scheme;
            uriBuilder.Host = request.Host.Host;
            if (request.Host.Port > 0)
                uriBuilder.Port = (int)request.Host.Port;
            uriBuilder.Path = request.Path.ToString();
            uriBuilder.Query = request.QueryString.ToString();

            return uriBuilder.Uri.AbsoluteUri;
        }

        public static string GetReferrer(this HttpRequest request, string defaultReferrer)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            string cachedReferrer = request.HasFormContentType && request.Form.ContainsKey(ReferrerConstants.Key)
                ? request.Form[ReferrerConstants.Key].ToString()
                : null;
            string referrer = string.IsNullOrWhiteSpace(cachedReferrer) == false ? cachedReferrer : defaultReferrer;
            return referrer;
        }
    }
}
