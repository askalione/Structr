using Microsoft.AspNetCore.Http;
using System;

namespace Structr.AspNetCore.Http
{
    /// <summary>
    /// Defines extension methods on <see cref="HttpRequest"/>.
    /// </summary>
    public static class HttpRequestExtensions
    {
        /// <summary>
        /// Determines if request has ajax nature.
        /// </summary>
        /// <param name="request"></param>
        /// <returns><see langword="true"/> if request is ajax request, otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return request.Headers["x-requested-with"] == "XMLHttpRequest";
        }

        /// <summary>
        /// Get an absolute Uri of request.
        /// </summary>
        /// <param name="request">Request to get uri from.</param>
        /// <returns>String representing absolute uri.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="request"/> is <see langword="null"/></exception>
        public static string GetAbsoluteUri(this HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            UriBuilder uriBuilder = new UriBuilder();
            uriBuilder.Scheme = request.Scheme;
            uriBuilder.Host = request.Host.Host;
            if (request.Host.Port > 0)
            {
                uriBuilder.Port = (int)request.Host.Port;
            }
            uriBuilder.Path = request.Path.ToString();
            uriBuilder.Query = request.QueryString.ToString();

            return uriBuilder.Uri.AbsoluteUri;
        }
    }
}
