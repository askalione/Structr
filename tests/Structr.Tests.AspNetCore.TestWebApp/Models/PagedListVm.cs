using Structr.Collections;

namespace Structr.Tests.AspNetCore.TestWebApp.Models
{
    public class PagedListVm
    {
        public PagedListQueryVm Query { get; }
        public IPagedEnumerable Items { get; }

        protected PagedListVm(PagedListQueryVm query, IPagedEnumerable items)
        {
            Query = query;
            Items = items;
        }
    }
}
