using Microsoft.AspNetCore.Mvc;
using Structr.AspNetCore.Http;
using System.Threading.Tasks;

namespace Structr.AspNetCore.JavaScript
{
    /// <summary>
    /// Represents the implementation of <see cref="RedirectResult"/> that checks that request has ajax nature
    /// and performs redirect to Url via ajax instead of normal redirecting. In case of normal request
    /// the standard redirecting procedure will be performed.
    /// </summary>
    public class AjaxRedirectResult : RedirectResult
    {
        /// <summary>
        /// Creates an instance of <see cref="AjaxRedirectResult">
        /// </summary>
        /// <param name="url">Url to redirect to.</param>
        public AjaxRedirectResult(string url)
            : base(url)
        { }

        public override async Task ExecuteResultAsync(ActionContext context)
        {
            if (context.HttpContext.Request.IsAjaxRequest())
            {
                var result = new JavaScriptResult($"window.location.replace('{Url}');");
                await result.ExecuteResultAsync(context);
            }
            else
            {
                await base.ExecuteResultAsync(context);
            }
        }
    }
}
