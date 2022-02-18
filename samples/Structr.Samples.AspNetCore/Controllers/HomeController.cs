using Microsoft.AspNetCore.Mvc;
using Structr.AspNetCore.Mvc;
using Structr.AspNetCore.Mvc.Attributes;
using Structr.Samples.AspNetCore.Extensions;
using Structr.Samples.AspNetCore.Models;
using System.Threading.Tasks;

namespace Structr.Samples.AspNetCore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Add javascript options
            // NOTE: Use IJavaScriptOptionProvider to get options. For example in TagHelper
            this.AddJavaScriptOptions(new
            {
                option1 = "value"
            });

            // Return alert with type Success
            // NOTE: Use IJavaScriptAlertProvider to get alerts. For example in TagHelper
            return View()
                .Success("Done");
        }

        // Permit ajax only request
        [AjaxOnly]
        public IActionResult JsonIndex()
        {
            // Use JsonResult(), JsonSuccess() and JsonError()
            return this.JsonSuccess("Foo message"); // Return { 'ok': true, 'message': 'Foo message' }
        }

        // Redirect ajax request
        public IActionResult RedirectAjaxIndex()
        {
            // If local redirect use LocalRedirectAjax(Url.Action("Index")) instead
            return this.RedirectAjax("http://google.com");
        }

        // Redirect to referrer
        public IActionResult RedirecToReferrerIndex()
        {
            return this.RedirectToReferrer(Url.Action("Index"));
        }

        // Permit ajax only request
        [AjaxOnly]
        public async Task<IActionResult> JsonPartialIndex()
        {
            // Use JsonResult(), JsonSuccess() and JsonError()
            var renderedPartialView = await this.RenderPartialViewAsync("Partial", new ViewModel());
            return this.JsonData(renderedPartialView); // Return { 'ok': true, 'data': '</>' }
        }
    }
}
