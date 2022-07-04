using Microsoft.AspNetCore.Http;
using System;

namespace Structr.AspNetCore.Referrer
{
    /// <summary>
    /// Defines extension methods on <see cref="HttpRequest"/>.
    /// </summary>
    public static class ReferrerHttpRequestExtensions
    {
        /// <summary>
        /// Get a string containing url gotten from <see cref="HttpRequest.Form"/> with <see cref="ReferrerConstants.Key"/> key.
        /// If there are no such key then the specified url will be returned instead.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="defaultReferrer">Fallback url to use in case of lack of <see cref="ReferrerConstants.Key"/> key in <see cref="HttpRequest.Form"/></param>
        /// <returns>Referrer or specified url.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <inheritdoc cref="ReferrerControllerExtensions.RedirectToReferrer(Microsoft.AspNetCore.Mvc.Controller, string)"/>
        public static string GetReferrer(this HttpRequest request, string defaultReferrer)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            string cachedReferrer = request.HasFormContentType && request.Form.ContainsKey(ReferrerDefaults.Key)
                ? request.Form[ReferrerDefaults.Key].ToString()
                : null;
            string referrer = string.IsNullOrWhiteSpace(cachedReferrer) == false ? cachedReferrer : defaultReferrer;
            return referrer;
        }
    }
}
