using Microsoft.AspNetCore.Mvc;
using Structr.AspNetCore.Mvc;
using Structr.Tests.AspNetCore.TestWebApp.Models;

namespace Structr.Tests.AspNetCore.TestWebApp.Controllers
{
    public class MvcControllerExtensionsController : Controller
    {
        public async Task<IActionResult> RenderPartialViewAsyncTest(int id, string name)
        {
            var model = new MvcControllerExtensionsTestVm { Id = id, Name = name };
            var result = await this.RenderPartialViewAsync("_RenderViewAsyncTest3Partial", model);
            return Content(result);
        }

        public async Task<IActionResult> RenderViewAsyncTest(int id, string name, string viewName, bool isPartial)
        {
            var model = new MvcControllerExtensionsTestVm { Id = id, Name = name };
            var result = await this.RenderViewAsync(viewName, model, isPartial);
            return Content(result);
        }
    }
}