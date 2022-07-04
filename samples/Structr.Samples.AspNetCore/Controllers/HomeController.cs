using Microsoft.AspNetCore.Mvc;
using Structr.AspNetCore.JavaScript;
using Structr.AspNetCore.Json;
using Structr.AspNetCore.Mvc;
using Structr.AspNetCore.Referrer;
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
            // NOTE: Use IClientOptionProvider to get options. For example in TagHelper
            this.AddClientOptions(new
            {
                option1 = "value"
            });

            // Return alert with type Success
            // NOTE: Use IClientAlertProvider to get alerts. For example in TagHelper
            return View()
                .Success("Done");
        }

        // Permit ajax only request
        [Ajax]
        public IActionResult JsonIndex()
        {
            // Use JsonResponse(), JsonSuccess() and JsonError()
            return this.JsonSuccess("Foo message"); // Return { 'ok': true, 'message': 'Foo message' }
        }

        // Redirect ajax request
        public IActionResult RedirectAjaxIndex()
        {
            // If local redirect use AjaxLocalRedirect(Url.Action("Index")) instead
            return this.AjaxRedirect("http://google.com");
        }

        // Redirect to referrer
        public IActionResult RedirecToReferrerIndex()
        {
            return this.RedirectToReferrer(Url.Action("Index"));
        }

        // Permit ajax only request
        [Ajax]
        public async Task<IActionResult> JsonPartialIndex()
        {
            // Use JsonResponse(), JsonSuccess() and JsonError()
            var renderedPartialView = await this.RenderPartialViewAsync("Partial", new ViewModel());
            return this.JsonResponse(ok: true, data: renderedPartialView); // Return { 'ok': true, 'data': '</>' }
        }
    }
}
