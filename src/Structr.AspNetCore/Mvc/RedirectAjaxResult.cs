using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Structr.AspNetCore.Mvc
{
    /// <summary>
    /// Represents the implimentation of <see cref="RedirectResult"/> that checks request has ajax nature
    /// and performs redirect to Url via ajax instead of normal redirecting. In case of normal request
    /// the standard redirecting procedure will be performed.
    /// </summary>
    public class RedirectAjaxResult : RedirectResult
    {
        /// <summary>
        /// Creates an instance of <see cref="RedirectAjaxResult">
        /// </summary>
        /// <param name="url"></param>
        public RedirectAjaxResult(string url)
            : base(url)
        { }

        public override async Task ExecuteResultAsync(ActionContext context)
        {
            if (context.HttpContext.Request.IsAjaxRequest())
            {
                JavaScriptResult result = new JavaScriptResult($"window.location.replace('{Url}');");
                await result.ExecuteResultAsync(context);
            }
            else
            {
                await base.ExecuteResultAsync(context);
            }
        }
    }
}
