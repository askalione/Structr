using Microsoft.AspNetCore.Mvc;
using Structr.Samples.Navigation.Attributes;

namespace Structr.Samples.Navigation.Controllers
{
    public class HomeController : Controller
    {
        [Menu("Child_1_1")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
