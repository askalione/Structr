using Microsoft.AspNetCore.Mvc;
using Structr.AspNetCore.JavaScript;

namespace Structr.Tests.AspNetCore.TestWebApp.Controllers
{
    public class AjaxRedirectResultController : Controller
    {
        public IActionResult ExecuteResultAsyncTest(string url)
        {
            var result = new AjaxRedirectResult(url);
            return result;
        }

        public IActionResult RedirectTarget()
        {
            return Ok();
        }
    }
}