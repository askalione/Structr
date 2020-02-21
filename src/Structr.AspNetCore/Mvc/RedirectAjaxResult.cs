using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Structr.AspNetCore.Mvc
{
    public class RedirectAjaxResult : RedirectResult
    {
        public RedirectAjaxResult(string url)
            : base(url)
        {

        }

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
