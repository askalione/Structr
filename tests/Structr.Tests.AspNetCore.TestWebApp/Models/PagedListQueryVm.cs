using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Structr.Tests.AspNetCore.TestWebApp.Models
{
    public class PagedListQueryVm : OrderedListQueryVm
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; }

        [BindNever]
        public IEnumerable<int> ItemsPerPage { get; set; } = new List<int>();
    }
}
