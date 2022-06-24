using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Structr.AspNetCore.Referrer
{
    /// <summary>
    /// Defines extension methods on <see cref="Controller"/>.
    /// </summary>
    public static class ReferrerControllerExtensions
    {
        /// <summary>
        /// Creates <see cref="RedirectResult"/> object specifying redirect to url depending on
        /// presence <see cref="ReferrerConstants.Key"/> key in <see cref="HttpRequest.Form"/>. In case of existing of such key
        /// the corresponding url from From value will be used to redirect to. In other case the
        /// provided <paramref name="url"/> will be.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="url">Fallback url to redirect to in case of lack of <see cref="ReferrerConstants.Key"/> key in <see cref="HttpRequest.Form"/></param>
        /// <returns>An instance of <see cref="RedirectResult"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <remarks>The extension method could be helpful when, for example, redirect back to entity's
        /// details form from edit form is needed after pressing "Ok" button or "Cancel" button. In such
        /// cases use of <see cref="TagHelpers.ReferrerTagHelper"/> will be helpful too.</remarks>
        public static RedirectResult RedirectToReferrer(this Controller controller, string url)
        {
            if (controller == null)
            {
                throw new ArgumentNullException(nameof(controller));
            }
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            var request = controller.HttpContext.Request;
            string referrer = request.HasFormContentType ? request.Form[ReferrerDefaults.Key].ToString() : "";
            string urlRedirect = string.IsNullOrEmpty(referrer) == false ? referrer : url;

            var redirect = new RedirectResult(urlRedirect);
            return redirect;
        }
    }
}
