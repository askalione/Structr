using Microsoft.AspNetCore.Mvc;
using Structr.AspNetCore.Mvc;

namespace Structr.Tests.AspNetCore.TestWebApp.Controllers
{
    public class RedirectAjaxResultController : Controller
    {
        public IActionResult ExecuteResultAsyncTest(string url)
        {
            var result = new RedirectAjaxResult(url);
            return result;
        }

        public IActionResult RedirectTarget()
        {
            return Ok();
        }
    }
}