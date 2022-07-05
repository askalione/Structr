using Microsoft.AspNetCore.Mvc;
using Structr.AspNetCore.TagHelpers;
using Structr.Collections;
using Structr.Tests.AspNetCore.TestWebApp.Models;

namespace Structr.Tests.AspNetCore.TestWebApp.Controllers
{
    public class TagHelpersController : Controller
    {
        public IActionResult PageSizeTagHelper()
        {
            var model = new FooPagedListVm
            {
                Query = new FooPagedListQueryVm
                {
                    ItemsPerPage = new List<int> { 3, 6, 10 },
                    Order = SortOrder.Asc,
                    PageNumber = 4,
                    //PageSize = 3,
                    Search = "e",
                    Sort = "Id"
                },
                Foos = new PagedList<FooVm>(
                    new List<FooVm>
                    {
                        new FooVm { Id = 10, Color = "Red" },
                        new FooVm { Id = 11, Color = "Blue" },
                        new FooVm { Id = 12, Color = "Green" }
                    },
                    25,
                    4,
                    3)
            };
            return View(model);
        }
    }
}