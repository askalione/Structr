using Structr.Collections;

namespace Structr.Tests.AspNetCore.TestWebApp.Models
{
    public class PagedListVm
    {
        public PagedListQueryVm Query { get; }
        public IPagedList Items { get; }

        protected PagedListVm(PagedListQueryVm query, IPagedList items)
        {
            Query = query;
            Items = items;
        }
    }
}
