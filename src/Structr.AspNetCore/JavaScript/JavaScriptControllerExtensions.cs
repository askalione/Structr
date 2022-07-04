using Microsoft.AspNetCore.Mvc;
using System;

namespace Structr.AspNetCore.JavaScript
{
    /// <summary>
    /// Defines extension methods on <see cref="Controller"/>.
    /// </summary>
    public static class JavaScriptControllerExtensions
    {
        /// <summary>
        /// Creates a <see cref="JavaScriptResult"/> with specified content.
        /// </summary>
        /// <param name="controller">The <see cref="Controller"/>.</param>
        /// <param name="content">Javascript code.</param>
        /// <returns>A <see cref="JavaScriptResult"/> object.</returns>
        public static JavaScriptResult JavaScript(this Controller controller, string content)
            => new JavaScriptResult(content);

        /// <summary>
        /// Creates an instance of <see cref="AjaxRedirectResult"/> while checking if url is local.
        /// In case of local url the redirect is performed to specified <paramref name="url"/>.
        /// Otherwise creates response for redirecting to application's root.
        /// </summary>
        /// <param name="controller">The <see cref="Controller"/>.</param>
        /// <param name="url">Url to redirect to.</param>
        /// <returns>An instace of <see cref="RedirectResult"/>.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="controller"/> is <see langword="null"/>.</exception>
        public static AjaxRedirectResult AjaxLocalRedirect(this Controller controller, string url)
        {
            if (controller == null)
            {
                throw new ArgumentNullException(nameof(controller));
            }

            return AjaxRedirect(controller, controller.Url.IsLocalUrl(url) ? url : "/");
        }

        /// <summary>
        /// Creates an instance of <see cref="AjaxRedirectResult"/>.
        /// </summary>
        /// <param name="controller">The <see cref="Controller"/>.</param>
        /// <param name="url">Url to redirect to.</param>
        /// <returns>An instace of <see cref="RedirectResult"/>.</returns>
        public static AjaxRedirectResult AjaxRedirect(this Controller controller, string url)
            => new AjaxRedirectResult(url);

    }
}
